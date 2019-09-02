using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set;}
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> pecasCapturadas;

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            pecas = new HashSet<Peca>();
            pecasCapturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            tab.validarPosicao(origem);
            tab.validarPosicao(destino);
            Peca p = tab.retirarPeca(origem);
            p.incrementarQrdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);

            if (pecaCapturada != null)
                pecasCapturadas.Add(pecaCapturada);

            tab.colocarPeca(p, destino);
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);
            turno++;
            mudaJogador();
        }

        public void validarPosOrigem(Posicao origem)
        {
            if (tab.peca(origem) == null)
                throw new TabuleiroException("Não existe peça na posição de origem!");
            if (jogadorAtual != tab.peca(origem).cor)
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            if (!tab.peca(origem).existeMovimentosPossiveis())
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
        }

        public void validarPosDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).podeMoverPara(destino))
                throw new TabuleiroException("Posicao de origem inválida!");

        }

        private void mudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
                jogadorAtual = Cor.Preta;
            else
                jogadorAtual = Cor.Branca;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        public void colocarPecas()
        {
            colocarNovaPeca('c', 1, new Torre(Cor.Preta, tab));
            colocarNovaPeca('a', 1, new Torre(Cor.Preta, tab));
            colocarNovaPeca('h', 1, new Torre(Cor.Preta, tab));
            colocarNovaPeca('b', 1, new Rei(Cor.Preta, tab));
            colocarNovaPeca('c', 8, new Torre(Cor.Branca, tab));
            colocarNovaPeca('a', 8, new Torre(Cor.Branca, tab));
            colocarNovaPeca('h', 8, new Torre(Cor.Branca, tab));
            colocarNovaPeca('b', 8, new Rei(Cor.Branca, tab));
        }

        public HashSet<Peca> pecasCapturadasPorCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca x in pecasCapturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecasCapturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadasPorCor(cor));
            return aux;
        }
    }
}
