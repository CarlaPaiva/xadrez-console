using System;
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
        public bool xeque { get; set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            pecas = new HashSet<Peca>();
            pecasCapturadas = new HashSet<Peca>();
            colocarPecas();
            xeque = false;
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            tab.validarPosicao(origem);
            tab.validarPosicao(destino);
            Peca p = tab.retirarPeca(origem);
            p.incrementarQrdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);

            if (pecaCapturada != null)
                pecasCapturadas.Add(pecaCapturada);

            return pecaCapturada;
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca capturada = executaMovimento(origem, destino);
            if (estaEmXeque(jogadorAtual))
            {
                defazMovimento(origem, destino, capturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
                xeque = true;
            else
                xeque = false;

            if (testeXequemate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }
        }

        public void defazMovimento(Posicao origem, Posicao destino, Peca capturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQrdMovimentos();
            if (capturada != null)
                pecasCapturadas.Remove(capturada);
            tab.colocarPeca(p, origem);
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
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadasPorCor(cor));
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
                return Cor.Preta;
            else
                return Cor.Branca;
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                    return x;
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca r = rei(cor);
            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[r.posicao.Linha, r.posicao.Coluna])
                    return true;
            }
            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
                return false;

            foreach (Peca x in pecas)
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i< tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Peca capturada = executaMovimento(origem, new Posicao(i, j));
                            bool testeXeque = estaEmXeque(cor);
                            defazMovimento(origem, new Posicao(i,j), capturada);
                            if (!testeXeque)
                                return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
