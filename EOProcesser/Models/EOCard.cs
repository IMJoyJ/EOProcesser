using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOProcesser.Models
{
    public class EOCard
    {
        public int CardId;
        public string CardName;

        public EOCard(int cardId, string cardName)
        {
            CardId = cardId;
            CardName = cardName;
        }

        public EOCard(EOCard card)
        {
            CardId = card.CardId;
            CardName = card.CardName;
        }

        public override string ToString()
        {
            return $"({CardId}){CardName}";
        }
    }
}
