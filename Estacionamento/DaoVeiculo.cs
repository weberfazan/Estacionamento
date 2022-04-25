using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;//Imports para conexão do Banco de Dados
using MySql.Data.MySqlClient;//Imports para realizar comandos no banco de dados

namespace Estacionamento
{
    class DaoVeiculo
    {
        MySqlConnection conexao;
        public string dados;
        public string resultado;
        public int[] cod;
        public string[] placa;
        public string[] modelo;
        public string[] cor;
        public string[] fabricante;
        public int i;
        public string msg;


        public int contadorLinhasTabela;


        //Construtor
        public DaoVeiculo()
        {
            conexao = new MySqlConnection("server=localhost;DataBase=Estacionamento;Uid=root;Password=;");
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
        public void Inserir(string placa, string modelo, string cor, string fabricante)
        {
            try
            {

                dados = "('','" + placa + "','" + modelo + "','" + cor + "','" + fabricante + "','" + "')";
                resultado = "Insert into veiculo(placa, modelo, cor, fabricante) values" + dados;
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
            string query = "select * from veiculo";//coletando dado do Banco de Dados

            //Instanciando os Vetores
            cod = new int[100];
            placa = new string[100];
            modelo = new string[100];
            cor = new string[100];
            fabricante = new string[100];


            //Dar valores iniciais para ele
            for (i = 0; i < 100; i++)
            {
                cod[i] = 0;
                placa[i] = "";
                modelo[i] = "";
                cor[i]  = "";
                fabricante[i] = "";

            }//fim da repetição

            //criar o comando para coletar dados
            MySqlCommand coletar = new MySqlCommand(query, conexao);
            //usar os comandos lendo os dados do banco de dados
            MySqlDataReader leitura = coletar.ExecuteReader();

            i = 0;
            while (leitura.Read())
            {
                cod[i] = Convert.ToInt32(leitura["codigo"]);
                placa[i] = leitura["placa"] + "";
                modelo[i] = leitura["modelo"] + "";
                cor[i] = leitura["cor"] + "";
                fabricante[i] = leitura["fabricante"] + "";
                i++;
                contadorLinhasTabela++;
            }//fim do metodo while

            //fechar o dataReader
            leitura.Close();

        }//fim do preencher vetor

        public string ConsultarIndividual(int codigo)
        {
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {

                if (codigo == cod[i])
                {
                    msg += "\n\nDados veículo\nCodigo:" + cod[i]
                                      + ",placa: " + placa[i]
                                      + ",modelo: " + modelo[i]
                                      + ",cor: " + cor[i]
                                      + ",fabricante: " + fabricante[i];

                    return msg;
                }

            }

            return "Código não encontrado!";
        }

        public string ConsultarTudo()
        {
            //Preencher o Vetor
            PreencherVetor();
            msg = "";
            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                msg += "\n\nDados veículo\nCodigo:" + cod[i]
                                      + ",placa: " + placa[i]
                                      + ",modelo: " + modelo[i]
                                      + ",cor: " + cor[i]
                                      + ",fabricante: " + fabricante[i];

            }//fim do for
            return msg;

        }//fim do consultar tudo

        public string Consultarplaca(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                if (codigo == cod[i])
                {
                    return placa[i];
                }//fim do if
            }//fim do for
            return "Codigo não encontrado!";
        }//fim do consultar placa

        public string Consultarmodelo(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                if (codigo == cod[i])
                {
                    return modelo[i];
                }//fim do if
            }//fim do for

            return "Codigo não encontrado!";

        }//fim do consultar modelo

        public string Consultarcor(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                if (codigo == cod[i])
                {
                    return cor[i];
                }//fim do if
            }//fim do for

            return "Codigo não encontrado!";

        }//fim do consultar cor

        public string Consultarfabricante(int codigo)
        {
            PreencherVetor();
            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                if (codigo == cod[i])
                {
                    return fabricante[i];
                }//fim do if
            }//fim do for

            return "Código não encontrado!";

        }//fim do consultar fabricante

        public void Atualizar(string campo, int codigo, string novoDado)
        {
            try
            {

            resultado = "update veiculo set" + campo + " = '" +
                         novoDado + "' where codigo = '" + codigo + "'";
            //Executar o script
            MySqlCommand sql = new MySqlCommand(resultado, conexao);
            resultado = "" + sql.ExecuteNonQuery();
            Console.WriteLine("Dado Atualizado com Sucesso!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!" + e);

            }// fim do catch
        }//fim do Atualizar

        public void Deletar(int codigo)
        {
            resultado = "delete from veiculo where codigo = '" + codigo + "'";
            //Executar o Comando
            MySqlCommand sql = new MySqlCommand(resultado, conexao);
            resultado = "" + sql.ExecuteNonQuery();
            //Mensagem...
            Console.WriteLine("Dados Excluido com sucesso!");
        }//fim do deletar

    }//fim da clase
}//fim do projeto
