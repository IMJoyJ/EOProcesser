using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOProcesser.Models
{
    internal class DeckEditorFile : IEnumerable<DeckEditorDeck>
    {
        public List<DeckEditorDeck> DeckEditorDecks = [];
        public List<ERACodeMultiLines> ExtraLines = [];
        public string DeckFile = "";

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
                    // コメント行と関数セグメントで新しいセグメントを作成
                    var segment = new ERACodeMultiLines();
                    segment.AddRange(commentLines);
                    segment.Add(funcSegment);
                    
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
                    
                    // 次のセグメント用にコメント行をクリア
                    commentLines.Clear();
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
                ExtraLines.Add([.. commentLines]);
            }
        }

        public IEnumerator<DeckEditorDeck> GetEnumerator()
        {
            return ((IEnumerable<DeckEditorDeck>)DeckEditorDecks).GetEnumerator();
        }

        public string ToFileContent()
        {
            ERACodeMultiLines lines = [.. ExtraLines];
            foreach (var deck in DeckEditorDecks)
            {
                lines.AddRange(deck.ToFunction());
            }
            return lines.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)DeckEditorDecks).GetEnumerator();
        }
    }
}
