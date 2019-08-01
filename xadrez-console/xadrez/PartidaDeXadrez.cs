﻿using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set;}
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            tab.validarPosicao(origem);
            tab.validarPosicao(destino);
            Peca p = tab.retirarPeca(origem);
            p.incrementarQrdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
        }

        public void colocarPecas()
        {
            tab.colocarPeca(new Torre(Cor.Preta, tab), new PosicaoXadrez('c', 1).toPosicao());
            tab.colocarPeca(new Torre(Cor.Preta, tab), new PosicaoXadrez('a', 1).toPosicao());
            tab.colocarPeca(new Torre(Cor.Preta, tab), new PosicaoXadrez('h', 1).toPosicao());
            tab.colocarPeca(new Rei(Cor.Preta, tab), new PosicaoXadrez('b', 1).toPosicao());
            tab.colocarPeca(new Torre(Cor.Branca, tab), new PosicaoXadrez('c', 8).toPosicao());
            tab.colocarPeca(new Torre(Cor.Branca, tab), new PosicaoXadrez('a', 8).toPosicao());
            tab.colocarPeca(new Torre(Cor.Branca, tab), new PosicaoXadrez('h', 8).toPosicao());
            tab.colocarPeca(new Rei(Cor.Branca, tab), new PosicaoXadrez('b', 8).toPosicao());
        }
    }
}
