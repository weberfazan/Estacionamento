using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;//Imports para conexão do Banco de Dados
using MySql.Data.MySqlClient;//Imports para realizar comandos no banco de dados

namespace Estacionamento
{
    class DaoFuncionario
    {
        MySqlConnection conexao;
        public string dados;
        public string resultado;
        public int[] cod;
        public string[] nome;
        public long[] CPF;
        public DateTime[] dataNascimento;
        public string[] telefone;
        public string[] funcao;
        public int i;
        public string msg;
        public int contador;
        public double[] salario;

        //Construtor
        public DaoFuncionario()
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
        public void Inserir(string nome, string telefone, DateTime dataNascimento, double salario, long CPF, string funcao)
        {
            try
            {
                dados = "('','" + nome + "','" + telefone + "','" + dataNascimento + "','" + salario + "','" + CPF + "','" + funcao + "')";
                resultado = "Insert into Pessoa(codigo, nome, telefone, dataNascimento, CPF, salario, funcao) values" + dados;
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
            CPF = new long[100];
            funcao = new string[100];
            dataNascimento = new DateTime[100];
            salario = new double[100];

            //Dar valores iniciais para ele
            for (i = 0; i < 100; i++)
            {
                cod[i] = 0;
                nome[i] = "";
                telefone[i] = "";
                dataNascimento[i] = new DateTime();
                salario[i] = 0;
                CPF[i] = 0;
                funcao[i] = "";
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
                CPF[i] = Convert.ToInt64(leitura ["CPF"]);
                dataNascimento[i] = Convert.ToDateTime(leitura["dataDeNascimento"]);
                salario[i] = Convert.ToDouble(leitura["valorPagamento"]);
                funcao[i] = leitura["funcao"] + "";
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
                                      + ",Nome: " + nome[i]
                                      + ",Telefone: " + telefone[i]
                                      + ",Data de Nascimento: " + dataNascimento[i]
                                      + ",Valor Pagamento: " + salario[i]
                                      + ",CPF: " + CPF[i]
                                      + "Função: " + funcao[i];
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

        public string Funcao(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contador; i++)
            {
                if (codigo == cod[i])
                {
                    return funcao[i];
                }//fim do if
            }//fim do for
            return "Codigo não encontrado!";
        }//fim do consultar função

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

        public double Salario(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contador; i++)
            {
                if (codigo == cod[i])
                {
                    return salario[i];
                }//fim do if
            }//fim do for
            return 00000.00;
        }//fim do consultar Salario

        public long cpf(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contador; i++)
            {
                if (codigo == cod[i])
                {
                    return CPF[i];
                }//fim do if
            }//fim do for
            return -1;
        }//fim do consultar CPF



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
