using System;
using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        public Rei(Cor cor, Tabuleiro tab) : base(cor, tab)
        {
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.cor != this.cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Posicao pos = new Posicao(0, 0);

            //acima
            pos.definirValores(posicao.Linha - 1, posicao.Coluna);

            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //nordeste
            pos.definirValores(posicao.Linha - 1, posicao.Coluna + 1);

            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //direita
            pos.definirValores(posicao.Linha, posicao.Coluna + 1);

            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //sudeste
            pos.definirValores(posicao.Linha + 1, posicao.Coluna + 1);

            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //abaixo
            pos.definirValores(posicao.Linha + 1, posicao.Coluna);

            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //so
            pos.definirValores(posicao.Linha + 1, posicao.Coluna - 1);

            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //esquerda
            pos.definirValores(posicao.Linha, posicao.Coluna - 1);

            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //noroeste
            pos.definirValores(posicao.Linha + 1, posicao.Coluna - 1);

            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            return mat;
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
