using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOProcesser.Models
{
    public class DeckEditorFile : IEnumerable<DeckEditorDeck>
    {
        public List<DeckEditorDeck> DeckEditorDecks = [];
        public List<ERACodeMultiLines> ExtraLines = [];
        public string DeckFile = "";

        public string GetExtraFunctionContent()
        {
            StringBuilder sb = new();
            foreach(var content in ExtraLines)
            {
                sb.AppendLine(content.ToString());
            }
            return sb.ToString();
        }

        public DeckEditorFile(string file)
        {
            DeckFile = file;
            // ファイルの内容を読み込んで解析
            ERACodeMultiLines fileContent = ERACodeAnalyzer.AnalyzeCode(File.ReadAllText(file));
            
            // コメント行を集めるための変数
            List<ERACodeCommentLine> commentLines = [];
            
            // ファイル内容を走査
            foreach (var code in fileContent)
            {
                if (code is ERACodeCommentLine commentLine)
                {
                    // コメント行を収集
                    commentLines.Add(commentLine);
                }
                else if (code is ERACodeFuncSegment funcSegment)
                {
                    // 関数セグメントの末尾にあるコメント行を抽出
                    List<ERACodeCommentLine> trailingComments = [];
                    List<ERACode> funcCodes = funcSegment.codes.ToList();
                    
                    // 末尾から逆順に走査し、連続するコメント行を見つける
                    for (int i = funcCodes.Count - 1; i >= 0; i--)
                    {
                        if (funcCodes[i] is ERACodeCommentLine trailingComment)
                        {
                            trailingComments.Insert(0, trailingComment); // 順序を保持するために先頭に挿入
                            funcCodes.RemoveAt(i); // 関数セグメントから削除
                        }
                        else
                        {
                            // コメント行でない要素に到達したら終了
                            break;
                        }
                    }
                    
                    // 関数セグメントからコメントを削除した新しいセグメントを作成
                    var cleanedFuncSegment = new ERACodeFuncSegment(funcSegment.FuncName);
                    cleanedFuncSegment.AddRange(funcCodes);
                    
                    // コメント行と関数セグメントで新しいセグメントを作成
                    var segment = new ERACodeMultiLines();
                    segment.AddRange(commentLines);
                    segment.Add(cleanedFuncSegment);
                    
                    // デッキとして初期化を試みる
                    try
                    {
                        var deck = new DeckEditorDeck(segment);
                        DeckEditorDecks.Add(deck);
                    }
                    catch (Exception)
                    {
                        // 失敗した場合は追加ラインとして追加
                        ExtraLines.Add(segment);
                    }
                    
                    // 次のセグメント用にコメント行を設定（関数末尾から抽出したコメント）
                    commentLines = trailingComments;
                }
                else if (!string.IsNullOrWhiteSpace(code.ToString().Trim()))
                {
                    // コメント行でも関数セグメントでもない非空白コードはエラー
                    throw new Exception("ファイル内に予期しないコードが見つかりました: " + code);
                }
            }
            
            // 残りのコメント行があれば追加
            if (commentLines.Count > 0)
            {
                ExtraLines.Add(new ERACodeMultiLines(commentLines));
            }
        }

        public IEnumerator<DeckEditorDeck> GetEnumerator()
        {
            return ((IEnumerable<DeckEditorDeck>)DeckEditorDecks).GetEnumerator();
        }

        public string ToFileContent()
        {
            StringBuilder sb = new();
            sb.AppendLine(GetExtraFunctionContent());
            foreach(var deck in DeckEditorDecks)
            {
                sb.AppendLine(deck.ToFunction().ToString());
            }
            return sb.ToString();
        }

        public void SaveToFile(string? file = null)
        {
            file ??= DeckFile;
            File.WriteAllText(file, ToFileContent());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)DeckEditorDecks).GetEnumerator();
        }
    }
}
