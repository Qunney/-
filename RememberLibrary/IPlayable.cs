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
    public interface IPlayable
    {
        void HideCard(int nr);
        void ShowCard(int nr, int card, int count);
        void ShowWinner();
    }
}
