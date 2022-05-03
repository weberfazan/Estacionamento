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
        public long[] CPF;
        public string[] nome;
        public DateTime[] dataDeNascimento;
        public string[] telefone;
        public double[] valor;
        public int contadorLinhasTabela;

        int opcao;

        //Construtor
        public DaoCliente()
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

        
        public void MenuCliente()
        {
            Console.WriteLine("\n\nEscolha uma das opções abaixo: \n\n" +
            "\n1. Cadastrar" +
            "\n2. Consultar Tudo" +
            "\n3. Consultar Individual" +
            "\n4. Atualizar" +
            "\n5. Excluir");
            opcao = Convert.ToInt32(Console.ReadLine());
        }

       
        public void ExecutarCliente()
        {

            do
            {
                MenuCliente();

                switch (opcao)
                {
                    case 1:
                        //Colentando os dados
                        Console.WriteLine("Informe seu nome: ");
                        string nome = Console.ReadLine();
                        Console.WriteLine("\nInforme seu telefone: ");
                        string telefone = Console.ReadLine();
                        Console.WriteLine("\nInforme DataDeNascimento: ");
                        DateTime DataDeNascimento= Convert.ToDateTime(Console.ReadLine());
                        Console.WriteLine("Informe o seu CPF:");
                        long CPF = (long)Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Informe valor:");
                        double valor = Convert.ToDouble(Console.ReadLine());

                        //Executar o método inserir
                        Inserir(CPF, nome, telefone, DataDeNascimento, valor);

                        break;

                    case 2:

                        Console.Clear();

                        ConsultarTudo();

                        break;

                    case 3:

                        Console.WriteLine("\nInsira o CPF que você quer consultar:");

                        CPF = (long)Convert.ToDouble(Console.Read());

                        Console.Clear();

                        ConsultarIndividual(CPF);
                       

                        break;

                    case 4:

                        
                                Console.WriteLine("\n\nDigite o CPF do Cliente");
                                CPF = (long)Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("Digite o campo que quer alterar:");
                                string campo = Console.ReadLine();
                                Console.WriteLine("Digite o novo dado");
                                string novoDado = Console.ReadLine();

                        Atualizar(campo, novoDado, CPF);


                        break;

                    case 5:

                        Console.WriteLine("Insira o CPF do Cliente que você quer excluir");
                        CPF = (long)Convert.ToDouble(Console.ReadLine());

                        Deletar(CPF);

                        break;

                    default:

                        Console.WriteLine("Digite um valor válido!");

                        break;
                }

            } while (opcao != 0);
        }

        //criar o metodo inserir
        public void Inserir(long CPF, string nome, string telefone, DateTime dataDeNascimento, double valor)
        {
            try
            {
                dados = $"('{CPF}','{nome}','{dataDeNascimento:yyyy-MM-dd}','{telefone}','{valor}')";
                resultado = "Insert into cliente(CPF, nome, dataDeNascimento, telefone, valor) values" + dados;
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
            string query = "select * from cliente";//coletando dado do Banco de Dados

            //Instanciando os Vetores
            CPF = new long[100];
            nome = new string[100];
            telefone = new string[100];
            dataDeNascimento = new DateTime[100];
            valor = new double[100];
            int i;

            //Dar valores iniciais para ele
            for (i = 0; i < 100; i++)
            {
                CPF[i] = 0;
                nome[i] = "";
                telefone[i] = "";
                dataDeNascimento[i] = new DateTime();
                valor[i] = 0;
            }//fim da repetição

            //criar o comando para coletar dados
            MySqlCommand coletar = new MySqlCommand(query, conexao);
            //usar os comandos lendo os dados do banco de dados
            MySqlDataReader leitura = coletar.ExecuteReader();

             i = 0;
            while (leitura.Read())
            {
                CPF[i] = (long)Convert.ToDouble(leitura["CPF"] + "");
                nome[i] = leitura["nome"] + "";
                telefone[i] = leitura["telefone"] + "";
                dataDeNascimento[i] = Convert.ToDateTime(leitura["dataDeNascimento"] + "");
                valor[i] = Convert.ToDouble( leitura["valor"] + ""); 
                
                i++;
                contadorLinhasTabela++;
            }//fim do metodo while

            //fechar o dataReader
            leitura.Close();

        }//fim do preencher vetor

        public void ConsultarIndividual(long documento)
        {
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {

                if (documento == CPF[i])
                {
                    Console.WriteLine($"CPF: {CPF[i]}\n" +
                                      $"Nome: {nome[i]}\n" +
                                      $"Telefone: {telefone[i]}\n" +
                                      $"Data de nascimento: {dataDeNascimento[i]:dd/MM/yyyy}\n" +
                                      $"Valor para pagamento: {valor[i]:C2}\n\n");
                }
                Console.WriteLine("Não encontrado!");
            }
        }

        public void ConsultarTudo()
        {
            //Preencher o Vetor
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                Console.WriteLine($"CPF: {CPF[i]}\n" +
                                  $"Nome: {nome[i]}\n" +
                                  $"Telefone: {telefone[i]}\n" +
                                  $"Data de nascimento: {dataDeNascimento[i]:dd/MM/yyyy}\n" +
                                  $"Valor para pagamento: {valor[i]:C2}\n\n");

            }//fim do for

        }//fim do consultar tudo

        public void Atualizar(string campo, string novoDado, long CPF)
        {
            try
            {
                resultado = $"update cliente set {campo} = '{novoDado}' where CPF = '{CPF}'";
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

        public void Deletar(long CPF)
        {
            resultado = $"delete from Cliente where CPF = '{CPF}'";
            //Executar o Comando
            MySqlCommand sql = new MySqlCommand(resultado, conexao);
            resultado = "" + sql.ExecuteNonQuery();
            //Mensagem...
            Console.WriteLine("Dados Excluido com sucesso!");
        }//fim do deletar


    }//fim da Classe Cliente

}//fim do projeto
