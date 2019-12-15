using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using xNet;
using System.IO;

namespace RememberLibrary
{
    public class LogicRemember
    {
        static Random rand = new Random();
        IPlayable play;
        int[] cards = new int[16];
        bool[] opens = new bool[16];
        int done;
        int status = 0;
        int card_a;
        int card_b;
        int count = 0;
        

        public LogicRemember(IPlayable play)
        {
            this.play = play;
        }
        public void CreateNewGame()
        {
            for (int j = 0; j < cards.Length; j++)
            {
                cards[j] = j % (cards.Length / 2) + 1;
            }
            for (int j = 0; j < 100; j++)
                shuffle_cards();
            for (int j = 0; j < cards.Length; j++)
                play.HideCard(j);
            for (int j = 0; j < cards.Length; j++)
                opens[j] = false;
            done = 0;
            status = 0;
            count = 0;
        }

        public void ClickPicture(int nr)
        {
            if (opens[nr]) return;
            switch (status)
            {
                case 0: status_0(nr); break;
                case 1: status_1(nr); break;
                case 2: status_2(nr); break;
                case 3: status_3(nr); break;

            }
        }

        private void shuffle_cards()
        {
            int a = rand.Next(0, cards.Length);
            int b = rand.Next(0, cards.Length);
            if (a == b) return;
            int x;
            x = cards[a];
            cards[a] = cards[b];
            cards[b] = x;
        }

        private void open(int picture)
        {
            opens[picture] = true;
            count++;
            play.ShowCard(picture, cards[picture], count);
        }

        private void status_0(int nr)
        {
            card_a = nr;
            count++;
            play.ShowCard(card_a, cards[card_a], count);
            status = 1;
        }

        private void status_1(int nr)
        {
            card_b = nr;
            if (card_a == card_b)
                return;
            count++;
            play.ShowCard(card_b, cards[card_b], count);
            
            status = 2;
            if (cards[card_a] == cards[card_b])
            {
                count--;
                open(card_a);
                count--;
                open(card_b);
                done += 2;
                if (done == 16)
                    play.ShowWinner();
                else status = 0;
            }
            else status = 3;
        }

        private void status_2(int nr)
        {

        }

        private void status_3(int nr)
        {
            play.HideCard(card_a);
            play.HideCard(card_b);
            card_a = nr;
            status_0(nr);
        }
       
    }
    
    

}
