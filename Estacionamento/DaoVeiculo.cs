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
        public string[] placa;
        public string[] modelo;
        public string[] cor;
        public string[] fabricante;
        public int i;


        public int contadorLinhasTabela;


        //Construtor
        public DaoVeiculo()
        {
            conexao = new MySqlConnection("server=localhost;DataBase=Estacionamentoti13n;Uid=root;Password=;Convert Zero Datetime=True");
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

                dados = $"('{placa}','{modelo}','{cor}','{fabricante}')";
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
            placa = new string[100];
            modelo = new string[100];
            cor = new string[100];
            fabricante = new string[100];


            //Dar valores iniciais para ele
            for (i = 0; i < 100; i++)
            {
                placa[i] = "";
                modelo[i] = "";
                cor[i] = "";
                fabricante[i] = "";

            }//fim da repetição

            //criar o comando para coletar dados
            MySqlCommand coletar = new MySqlCommand(query, conexao);
            //usar os comandos lendo os dados do banco de dados
            MySqlDataReader leitura = coletar.ExecuteReader();

            i = 0;
            while (leitura.Read())
            {
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

        public void ConsultarIndividual(string pla)
        {
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {

                if (pla == placa[i])
                {
                    Console.WriteLine($"\n\nDados veículo\n" +
                           $"Placa: {placa[i]}\n" +
                           $"Modelo: {modelo[i]}\n" +
                           $"Cor: {cor}\n" +
                           $"Fabricante: {fabricante}\n");
                }

                Console.WriteLine("Código não encontrado!");

            }
        }

        public void ConsultarTudo()
        {
            //Preencher o Vetor
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                Console.WriteLine($"\n\nDados veículo\n" +
                           $"Placa: {placa[i]}\n" +
                           $"Modelo: {modelo[i]}\n" +
                           $"Cor: {cor}\n" +
                           $"Fabricante: {fabricante}\n");

            }//fim do for

        }//fim do consultar tudo

        public void Atualizar(string campo, string pla, string novoDado)
        {
            try
            {

                resultado = $"update veiculo set {campo} = '{novoDado}' where placa = {pla}";
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

        public void Deletar(string pla)
        {
            resultado = $"delete from veiculo where placa = '{pla}'";
            //Executar o Comando
            MySqlCommand sql = new MySqlCommand(resultado, conexao);
            resultado = "" + sql.ExecuteNonQuery();
            //Mensagem...
            Console.WriteLine("Dados Excluido com sucesso!");
        }//fim do deletar

    }//fim da clase
}//fim do projeto
