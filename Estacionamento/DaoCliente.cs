using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;//Imports para conexão do Banco de Dados
using MySql.Data.MySqlClient;//Imports para realizar comandos no banco de dados

namespace Estacionamento
{
    class DaoCliente
    {
        MySqlConnection conexao;
        public string dados;
        public string resultado;
        public int[] cod;
        public string[] nome;
        public DateTime[] dataNascimento;
        public string[] telefone;        
        public int i;
        public string msg;
        public int contador;
        public double[] valorPagamento;

        //Construtor
        public DaoCliente()
        {
            conexao = new MySqlConnection("server=localhost;DataBase=BancoDeDadosTI13N;Uid=root;Password=;");
            try
            {
                conexao.Open();//solicitando a entrada do banco de dados                
            }
            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!!!\n\n" + e);
                conexao.Close();//fechando a conexão com o danco de dados
            }//fim da tentativa de conexão com o banco de dados
        }//fim do metodo construtor

        //criar o metodo inserir
        public void Inserir(string nome, string telefone, DateTime dataNascimento, double valorPagamento)
        {
            try
            {
                dados = "('','" + nome + "','" + telefone + "','" + dataNascimento + "','" + valorPagamento + "')";
                resultado = "Insert into Pessoa(codigo, nome, telefone, dataNascimento, valorPagamento) values" + dados;
                //Executar o comando resultado no banco de dados
                MySqlCommand sql = new MySqlCommand(resultado, conexao);
                resultado = "" + sql.ExecuteNonQuery();
                Console.WriteLine(resultado + "Linha(s) Afetadas!");

            }//fim do try

            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!\n\n" + e);

            }//fim do catch

        }//fim do metodo Inserir

        public void PreencherVetor()
        {
            string query = "select * from pessoa";//coletando dado do Banco de Dados

            //Instanciando os Vetores
            cod = new int[100];
            nome = new string[100];
            telefone = new string[100];
            dataNascimento = new DateTime[100];
            valorPagamento = new double[100];

            //Dar valores iniciais para ele
            for (i = 0; i < 100; i++)
            {
                cod[i] = 0;
                nome[i] = "";
                telefone[i] = "";
                dataNascimento[i] = new DateTime() ;
                valorPagamento[i] = 0;
            }//fim da repetição

            //criar o comando para coletar dados
            MySqlCommand coletar = new MySqlCommand(query, conexao);
            //usar os comandos lendo os dados do banco de dados
            MySqlDataReader leitura = coletar.ExecuteReader();

            i = 0;
            while (leitura.Read())
            {
                cod[i] = Convert.ToInt32(leitura["codigo"]);
                nome[i] = leitura["nome"] + "";
                telefone[i] = leitura["telefone"] + "";
                dataNascimento[i] = Convert.ToDateTime(leitura["dataDeNascimento"]);
                valorPagamento[i] = Convert.ToDouble( leitura["valorPagamento"]); 
                i++;
                contador++;
            }//fim do metodo while

            //fechar o dataReader
            leitura.Close();

        }//fim do preencher vetor

        public string ConsultarTudo()
        {
            //Preencher o Vetor
            PreencherVetor();
            msg = "";
            for (int i = 0; i < contador; i++)
            {
                msg += "\n\n Codigo:" + cod[i]
                                      + ",Nome:" + nome[i]
                                      + ",Telefone:" + telefone[i]
                                      + ",Data de Nascimento: " + dataNascimento[i]
                                      + "Valor Pagamento: " + valorPagamento[i];
            }//fim do for
            return msg;

        }//fim do consultar tudo

        public string ConsultarNome(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contador; i++)
            {
                if (codigo == cod[i])
                {
                    return nome[i];
                }//fim do if
            }//fim do for
            return "Codigo não encontrado!";
        }//fim do consultar nome

        public string ConsultarTelefone(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contador; i++)
            {
                if (codigo == cod[i])
                {
                    return telefone[i];
                }//fim do if
            }//fim do for
            return "Codigo não encontrado!";
        }//fim do consultar telefone

        public DateTime ConsultarDataNascimento(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contador; i++)
            {
                if (codigo == cod[i])
                {
                    return dataNascimento[i];
                }//fim do if
            }//fim do for
            return new DateTime();
        }//fim do consultar dataNascimento

        public double ValorPagamento(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contador; i++)
            {
                if (codigo == cod[i])
                {
                    return valorPagamento[i];
                }//fim do if
            }//fim do for
            return 00000.00;
        }//fim do consultar Valor Pagamento

        public void Atualizar(string campo, string novoDado, int codigo)
        {
            try
            {
                resultado = "update pessoa set" + campo + " = '" +
                             novoDado + "' where codigo '" + codigo + "'";
                //Executar o script
                MySqlCommand sql = new MySqlCommand(resultado, conexao);
                resultado = "" + sql.ExecuteNonQuery();
                Console.WriteLine("Dado Atualizado com Sucesso!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!" + e);

            }


        }//fim do Atualizar

        public void Deletar(int codigo)
        {
            resultado = "delete from pessoa where codigo = '" + codigo + "'";
            //Executar o Comando
            MySqlCommand sql = new MySqlCommand(resultado, conexao);
            resultado = "" + sql.ExecuteNonQuery();
            //Mensagem...
            Console.WriteLine("Dados Excluido com sucesso!");
        }//fim do deletar


    }//fim da Classe Cliente

}//fim do projeto
