using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EOProcesser
{
    public static class Utils
    {
        private static readonly string[] separator = ["\r\n", "\n"];

        public static string TrimCode(string code)
        {
            string[] lines = code.Split(separator, StringSplitOptions.None);
            StringBuilder result = new();
            bool isSkipSection = false;
            bool previousLineWasEmpty = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                // Check if this is a skip section marker
                if (line.Trim() == "[SKIPSTART]")
                {
                    isSkipSection = true;
                    result.AppendLine(line);
                    continue;
                }
                else if (line.Trim() == "[SKIPEND]")
                {
                    isSkipSection = false;
                    result.AppendLine(line);
                    continue;
                }

                // Handle regular lines
                if (isSkipSection)
                {
                    // In skip section, preserve all lines as-is
                    result.AppendLine(line);
                }
                else
                {
                    bool isCurrentLineEmpty = string.IsNullOrWhiteSpace(line);

                    // If line is empty and previous line was also empty, skip it
                    if (isCurrentLineEmpty && previousLineWasEmpty)
                    {
                        continue;
                    }

                    // Trim empty lines, keep non-empty lines as-is
                    if (isCurrentLineEmpty)
                    {
                        result.AppendLine("");
                    }
                    else
                    {
                        result.AppendLine(line);
                    }

                    previousLineWasEmpty = isCurrentLineEmpty;
                }
            }

            return result.ToString();
        }

        public static void InitTreeViewByFolder(string folder, TreeView treeView, Action<TreeNode, bool>? whenGeneration = null, string searchPattern = "*.*")
        {
            if (!Directory.Exists(folder))
                return;

            treeView.Nodes.Clear();

            // 直接将目录内容添加到TreeView的根节点集合
            PopulateDirectoryContents(treeView.Nodes, folder, whenGeneration, searchPattern);
        }

        private static void PopulateDirectoryContents(TreeNodeCollection nodes, string folderPath, Action<TreeNode, bool>? whenGeneration, string searchPattern)
        {
            try
            {
                // 添加子目录
                foreach (string directory in Directory.GetDirectories(folderPath))
                {
                    string dirName = Path.GetFileName(directory);
                    TreeNode dirNode = new(dirName)
                    {
                        Tag = directory
                    };
                    nodes.Add(dirNode);

                    // 递归处理子目录
                    PopulateTreeNode(dirNode, directory, whenGeneration, searchPattern);

                    // 子目录的所有节点都已生成，执行回调
                    whenGeneration?.Invoke(dirNode, true);
                }

                // 添加符合条件的文件
                foreach (string file in Directory.GetFiles(folderPath, searchPattern))
                {
                    string fileName = Path.GetFileName(file);
                    TreeNode fileNode = new(fileName)
                    {
                        Tag = file
                    };
                    nodes.Add(fileNode);

                    // 文件节点生成后，立即执行回调
                    whenGeneration?.Invoke(fileNode, false);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // 处理访问权限错误
                TreeNode errorNode = new("访问被拒绝");
                nodes.Add(errorNode);
            }
            catch (Exception ex)
            {
                // 处理其他异常
                TreeNode errorNode = new($"错误: {ex.Message}");
                nodes.Add(errorNode);
            }
        }

        private static void PopulateTreeNode(TreeNode parentNode, string folderPath, Action<TreeNode, bool>? whenGeneration, string searchPattern)
        {
            try
            {
                // 添加子目录
                foreach (string directory in Directory.GetDirectories(folderPath))
                {
                    string dirName = Path.GetFileName(directory);
                    TreeNode dirNode = new(dirName)
                    {
                        Tag = directory
                    };
                    parentNode.Nodes.Add(dirNode);

                    // 递归处理子目录
                    PopulateTreeNode(dirNode, directory, whenGeneration, searchPattern);

                    // 子目录的所有节点都已生成，执行回调
                    whenGeneration?.Invoke(dirNode, true);
                }

                // 添加符合条件的文件
                foreach (string file in Directory.GetFiles(folderPath, searchPattern))
                {
                    string fileName = Path.GetFileName(file);
                    TreeNode fileNode = new(fileName)
                    {
                        Tag = file
                    };
                    parentNode.Nodes.Add(fileNode);

                    // 文件节点生成后，立即执行回调
                    whenGeneration?.Invoke(fileNode, false);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // 处理访问权限错误
                TreeNode errorNode = new("（エラー：読み込む権限がありません）");
                parentNode.Nodes.Add(errorNode);
            }
            catch (Exception ex)
            {
                // 处理其他异常
                TreeNode errorNode = new($"エラー: {ex.Message}");
                parentNode.Nodes.Add(errorNode);
            }
        }
        // 缓存加粗字体，避免重复创建导致 GDI 句柄泄漏
        private static readonly Dictionary<Font, Font> _boldFontCache = [];

        private static Font GetBoldFont(Font baseFont)
        {
            if (!_boldFontCache.TryGetValue(baseFont, out var bold))
            {
                bold = new Font(baseFont, FontStyle.Bold);
                _boldFontCache[baseFont] = bold;
            }
            return bold;
        }
        /// <summary>
        /// 在 TreeView 中搜索并高亮命中节点，展开必要的分支，未命中节点恢复常规样式。
        /// 空或空白 searchPattern 表示清除高亮与展开（全部折叠）。
        /// </summary>
        public static void SearchTreeViewWithString(string searchPattern,
            TreeView treeView, Func<TreeNode, string, bool>? judgeFunc = null)
        {
            ArgumentNullException.ThrowIfNull(treeView);

            judgeFunc ??= (t, str) => t.Text.Contains(str, StringComparison.OrdinalIgnoreCase);

            string pattern = searchPattern ?? string.Empty;
            bool hasPattern = !string.IsNullOrWhiteSpace(pattern);
            pattern = pattern.Trim();

            treeView.SuspendLayout();
            try
            {
                // 第一遍：复位所有节点样式
                foreach (TreeNode root in treeView.Nodes)
                {
                    ResetNodeStyleRecursive(root, treeView);
                }

                if (!hasPattern)
                {
                    // 清除搜索：可选择把所有节点折叠
                    foreach (TreeNode root in treeView.Nodes)
                    {
                        root.Collapse();
                    }
                    return;
                }

                // 第二遍：搜索 + 高亮 + 控制展开
                foreach (TreeNode root in treeView.Nodes)
                {
                    ProcessNode(root, pattern, treeView, judgeFunc, out _);
                }
            }
            finally
            {
                treeView.ResumeLayout();
            }
        }

        /// <summary>
        /// 递归处理节点，返回当前节点或其子孙是否有匹配，用于决定展开。
        /// </summary>
        private static bool ProcessNode(TreeNode node, string pattern, TreeView treeView,
            Func<TreeNode, string, bool> judgeFunc, out bool selfMatched)
        {
            selfMatched = judgeFunc(node, pattern);

            bool anyDescendantMatched = false;
            foreach (TreeNode child in node.Nodes)
            {
                bool childMatched = ProcessNode(child, pattern, treeView, judgeFunc, out bool childSelfMatched);
                if (childMatched) anyDescendantMatched = true;
            }

            bool matched = selfMatched || anyDescendantMatched;

            // 设置样式
            if (selfMatched)
            {
                node.BackColor = Color.Yellow;
                node.ForeColor = Color.Black;
                node.NodeFont = GetBoldFont(treeView.Font);
            }
            else
            {
                // 未命中自身（但可能后代命中），自身保持普通样式
                node.BackColor = treeView.BackColor;
                node.ForeColor = treeView.ForeColor;
                node.NodeFont = treeView.Font; // 或设为 null 继承父级
            }

            // 展开/折叠逻辑：只有匹配路径才展开
            if (matched)
                node.Expand();
            else
                node.Collapse();

            return matched;
        }

        /// <summary>
        /// 递归恢复节点样式到默认。
        /// </summary>
        private static void ResetNodeStyleRecursive(TreeNode node, TreeView treeView)
        {
            node.BackColor = treeView.BackColor;
            node.ForeColor = treeView.ForeColor;
            node.NodeFont = treeView.Font; // 或 null
            foreach (TreeNode child in node.Nodes)
            {
                ResetNodeStyleRecursive(child, treeView);
            }
        }// 原始项快照
        private static readonly Dictionary<ListBox, List<object>> _listBoxOriginalItems = [];
        // 是否原本是 DataSource 绑定
        private static readonly Dictionary<ListBox, bool> _listBoxWasDataBound = [];
        // 原始 DataSource 引用
        private static readonly Dictionary<ListBox, object?> _listBoxOriginalDataSource = [];
        // 原始 DisplayMember / ValueMember
        private static readonly Dictionary<ListBox, string?> _listBoxOriginalDisplayMember = [];
        private static readonly Dictionary<ListBox, string?> _listBoxOriginalValueMember = [];
        /// <summary>
        /// 针对 ListBox 的搜索过滤：
        /// 1. 首次调用会拍下原始所有项（无论是否 DataSource 模式）；
        ////   - 若原先为 DataSource：保存 DataSource / DisplayMember / ValueMember，并切换到 Items 模式以显示过滤结果
        /// 2. 之后每次搜索都基于首次快照，不做“叠加过滤”；
        /// 3. searchPattern 为空或全空白 => 还原：
        ///    - 若初始为 DataSource => 恢复 DataSource 绑定；
        ///    - 否则 => 恢复原 Items；
        /// 4. 默认 judgeFunc：item.ToString() 大小写不敏感包含；
        /// 5. 仅对能转换为 T 的项执行 judgeFunc，其余项在过滤状态下不显示（可按需改动）
        /// 6. 返回匹配的 T 集合（清空搜索时返回全部 T）
        /// </summary>
        public static List<T> SearchListBoxWithString<T>(
            string searchPattern,
            ListBox listBox,
            Func<T, string, bool>? judgeFunc = null)
        {
            ArgumentNullException.ThrowIfNull(listBox);

            // 首次初始化缓存
            if (!_listBoxOriginalItems.TryGetValue(listBox, out var originalObjects))
            {
                originalObjects = [];

                bool wasDataBound = listBox.DataSource != null;
                _listBoxWasDataBound[listBox] = wasDataBound;

                if (wasDataBound)
                {
                    // 保存原数据绑定信息
                    _listBoxOriginalDataSource[listBox] = listBox.DataSource;
                    _listBoxOriginalDisplayMember[listBox] = listBox.DisplayMember;
                    _listBoxOriginalValueMember[listBox] = listBox.ValueMember;

                    if (listBox.DataSource is System.Collections.IEnumerable ds)
                    {
                        foreach (var o in ds)
                            originalObjects.Add(o!);
                    }
                    else
                    {
                        // 罕见情况：DataSource 不可枚举（几乎不会发生）
                    }

                    // 切换为 Items 模式以便我们控制过滤内容
                    listBox.DataSource = null;
                    listBox.Items.Clear();
                    foreach (var o in originalObjects)
                        listBox.Items.Add(o);
                }
                else
                {
                    foreach (var o in listBox.Items)
                        originalObjects.Add(o!);
                }

                _listBoxOriginalItems[listBox] = originalObjects;
            }

            string pattern = searchPattern ?? string.Empty;
            bool hasPattern = !string.IsNullOrWhiteSpace(pattern);
            pattern = pattern.Trim();

            judgeFunc ??= (item, p) =>
            {
                var text = item?.ToString() ?? string.Empty;
                return text.Contains(p, StringComparison.OrdinalIgnoreCase);
            };

            var wasDataBoundOriginal = _listBoxWasDataBound.TryGetValue(listBox, out var wdb) && wdb;

            listBox.BeginUpdate();
            try
            {
                // 清空搜索 => 恢复
                if (!hasPattern)
                {
                    if (wasDataBoundOriginal)
                    {
                        // 恢复数据绑定
                        listBox.Items.Clear();
                        if (_listBoxOriginalDataSource.TryGetValue(listBox, out var dsObj))
                        {
                            listBox.DataSource = dsObj;
                            // 恢复 DisplayMember / ValueMember
                            if (_listBoxOriginalDisplayMember.TryGetValue(listBox, out var dm))
                                listBox.DisplayMember = dm ?? string.Empty;
                            if (_listBoxOriginalValueMember.TryGetValue(listBox, out var vm))
                                listBox.ValueMember = vm ?? string.Empty;
                        }
                    }
                    else
                    {
                        // 非 DataSource 模式：直接恢复 Items
                        listBox.DataSource = null; // 保守写法
                        listBox.Items.Clear();
                        foreach (var o in originalObjects)
                            listBox.Items.Add(o);
                    }

                    // 返回所有可转换为 T 的项目
                    var restored = new List<T>();
                    foreach (var o in originalObjects)
                    {
                        if (o is T t) restored.Add(t);
                    }
                    return restored;
                }

                // 执行过滤：若最初是 DataSource 模式，此时我们已经处于 Items 模式
                listBox.DataSource = null; // 确保使用 Items 方式显示过滤结果
                listBox.Items.Clear();

                var matched = new List<T>();
                foreach (var o in originalObjects)
                {
                    if (o is T tItem)
                    {
                        if (judgeFunc(tItem, pattern))
                        {
                            matched.Add(tItem);
                            listBox.Items.Add(o);
                        }
                    }
                    // 不可转换为 T 的项若需要保留，可在此处考虑：
                    // else listBox.Items.Add(o);
                }

                return matched;
            }
            finally
            {
                listBox.EndUpdate();
            }
        }

    }
}