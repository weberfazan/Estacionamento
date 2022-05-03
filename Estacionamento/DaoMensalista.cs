using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;//Imports para conexão do Banco de Dados
using MySql.Data.MySqlClient;//Imports para realizar comandos no banco de dados

namespace Estacionamento
{
    class DaoMensalista
    {
        MySqlConnection conexao;
        DaoVeiculo daoVeiculo;

        public string dados;
        public string resultado;
        public int[] cod;
        public string[] nome;
        public long[] CPF;
        public DateTime[] dataDePagamento;
        public string[] telefone;
        public string[] endereco;
        public int i;
        public string msg;
        public int opcao;
        public int contadorLinhasTabela;
        public double[] valorMensal;

        //Construtor
        public DaoMensalista()
        {
            conexao = new MySqlConnection("server=localhost;DataBase=Estacionamentoti13n;Uid=root;Password=;Convert Zero Datetime=True");
            daoVeiculo = new DaoVeiculo();

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


        public void MostrarOpcoesMensalista()
        {
            Console.WriteLine("\n\nEscolha uma das opções abaixo: \n\n" +
            "\n1. Cadastrar" +
            "\n2. Consultar Tudo" +
            "\n3. Consultar Individual" +
            "\n4. Atualizar" +
            "\n5. Excluir" +
            "\n0. Voltar");
            opcao = Convert.ToInt32(Console.ReadLine());
        }//fim do método

        public void ExecutarMensalista()
        {

            do
            {
                MostrarOpcoesMensalista();

                switch (opcao)
                {
                    case 1:
                        //Colentando os dados
                        Console.WriteLine("Informe seu nome: ");
                        string nome = Console.ReadLine();
                        Console.WriteLine("\nInforme seu telefone: ");
                        string telefone = Console.ReadLine();
                        Console.WriteLine("\nInforme a data de pagamento: ");
                        DateTime dataDePagamento = Convert.ToDateTime(Console.ReadLine());
                        Console.WriteLine("\nInforme valorPagamento: ");
                        double valorPagamento = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Informe o seu CPF:");
                        long CPF = (long)Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Informe o seu endereço:");
                        string endereco = Console.ReadLine();
                        Console.WriteLine("Informe a cor do carro:");
                        string cor = Console.ReadLine();
                        Console.WriteLine("Informe a placa do carro:");
                        string placa = Console.ReadLine();
                        Console.WriteLine("Informe o modelo do carro:");
                        string modelo = Console.ReadLine();
                        Console.WriteLine("Informe o fabricante do carro:");
                        string fabricante = Console.ReadLine();

                        //Executar o método inserir
                        Inserir(nome, telefone, dataDePagamento, valorPagamento, CPF, endereco, cor, placa, modelo, fabricante);

                        break;

                    case 2:

                        ConsultarTudo();

                        break;

                    case 3:

                        Console.WriteLine("\nInsira o código que você quer consultar:");

                        int codigo = Convert.ToInt32(Console.Read());

                        Console.WriteLine(ConsultarIndividual(codigo));

                        break;

                    case 4:

                        bool validador = false;
                        do
                        {
                            Console.WriteLine("\nQuer atualizar um veículo ou um mensalista?\n1.Veículo\n2.Mensalista");
                            int opcao = Convert.ToInt32(Console.ReadLine());

                            if (opcao == 1)
                            {
                                Console.WriteLine("\n\nDigite a placa do veículo");
                                placa = Console.ReadLine();
                                Console.WriteLine("Digite o campo que quer alterar:");
                                string campo = Console.ReadLine();
                                Console.WriteLine("Digite o novo dado");
                                string novoDado = Console.ReadLine();
                                daoVeiculo.Atualizar(campo, placa, novoDado);

                                validador = true;
                            }
                            else
                            {
                                if (opcao == 2)
                                {
                                    Console.WriteLine("\n\nDigite o código do mensalista");
                                    codigo = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Digite o campo que quer alterar:");
                                    string campo = Console.ReadLine();

                                    if (campo.ToLower() == "datadepagamento")
                                    {
                                        Console.WriteLine("Digite a nova data:");
                                        DateTime data = Convert.ToDateTime(Console.ReadLine());
                                        AtualizarData(campo, data, codigo);

                                        validador = true;
                                    }
                                    else
                                    {

                                        Console.WriteLine("Digite o novo dado");
                                        string novoDado = Console.ReadLine();
                                        Atualizar(campo, codigo, novoDado);

                                        validador = true;
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Insira um valor válido!");
                                }
                            }
                        } while (validador != true);


                        break;

                    case 5:

                        Console.WriteLine("Insira o código do mensalista que você quer excluir");
                        codigo = Convert.ToInt32(Console.ReadLine());

                        Deletar(codigo);

                        Console.WriteLine("Digite a placa do carro que quer excluir:");
                        placa = Console.ReadLine();

                        daoVeiculo.Deletar(placa);

                        break;

                    case 0:

                        Console.Clear();
                        break;

                    default:

                        Console.WriteLine("Digite um valor válido!");

                        break;
                }

            } while (opcao != 0);
        }



        //criar o metodo inserir
        public void Inserir(string nome, string telefone, DateTime dataDePagamento, double valorMensal, long CPF, string endereco, string cor, string placa, string modelo, string fabricante)
        {
            try
            {
                dados = $"('','{nome}','{CPF}','{endereco}','{telefone}','{dataDePagamento:yyyy-MM-dd}','{valorMensal}')";
                resultado = "Insert into mensalidade(codigo, nome, CPF, endereco, telefone, dataDePagamento, valormensal) values" + dados;
                //Executar o comando resultado no banco de dados
                MySqlCommand sql = new MySqlCommand(resultado, conexao);
                resultado = "" + sql.ExecuteNonQuery();
                Console.WriteLine(resultado + "Linha(s) Afetadas!");

                daoVeiculo.Inserir(placa, modelo, cor, fabricante);

            }//fim do try

            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!\n\n" + e);

            }//fim do catch

        }//fim do metodo Inserir

        public void PreencherVetor()
        {
            string query = "select * from mensalidade";//coletando dado do Banco de Dados

            //Instanciando os Vetores
            cod = new int[100];
            nome = new string[100];
            telefone = new string[100];
            CPF = new long[100];
            endereco = new string[100];
            dataDePagamento = new DateTime[100];
            valorMensal = new double[100];
            daoVeiculo.cor = new string[100];
            daoVeiculo.fabricante = new string[100];
            daoVeiculo.modelo = new string[100];
            daoVeiculo.placa = new string[100];

            //Dar valores iniciais para ele
            for (i = 0; i < 100; i++)
            {
                cod[i] = 0;
                nome[i] = "";
                telefone[i] = "";
                dataDePagamento[i] = new DateTime();
                valorMensal[i] = 0;
                CPF[i] = 0;
                endereco[i] = "";
                daoVeiculo.cor[i] = "";
                daoVeiculo.fabricante[i] = "";
                daoVeiculo.modelo[i] = "";
                daoVeiculo.placa[i] = "";

            }//fim da repetição

            //criar o comando para coletar dados
            MySqlCommand coletar = new MySqlCommand(query, conexao);
            //usar os comandos lendo os dados do banco de dados
            MySqlDataReader leitura = coletar.ExecuteReader();

            i = 0;
            contadorLinhasTabela = 0;

            while (leitura.Read())
            {
                cod[i] = Convert.ToInt32(leitura["codigo"] + "");
                nome[i] = leitura["nome"] + "";
                telefone[i] = leitura["telefone"] + "";
                CPF[i] = (long)Convert.ToDouble(leitura["CPF"] + "");
                dataDePagamento[i] = Convert.ToDateTime(leitura["dataDePagamento" + ""]);
                valorMensal[i] = Convert.ToDouble(leitura["valormensal"] + "");
                endereco[i] = leitura["endereco"] + "";

                i++;
                contadorLinhasTabela++;
            }//fim do metodo while

            //fechar o dataReader
            leitura.Close();


            query = "select * from veiculo";//coletando dado do Banco de Dados

            //criar o comando para coletar dados
            coletar = new MySqlCommand(query, conexao);
            //usar os comandos lendo os dados do banco de dados
            leitura = coletar.ExecuteReader();

            i = 0;
            while (leitura.Read())
            {
                daoVeiculo.placa[i] = leitura["placa"] + "";
                daoVeiculo.modelo[i] = leitura["modelo"] + "";
                daoVeiculo.cor[i] = leitura["cor"] + "";
                daoVeiculo.fabricante[i] = leitura["fabricante"] + "";

                i++;

            }//fim do metodo while

            //fechar o dataReader
            leitura.Close();

        }//fim do preencher vetor

        // Método para consultar individual

        public string ConsultarIndividual(int codigo)
        {
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {

                if (codigo == cod[i])
                {
                    msg += "\n\n Codigo:" + cod[i]
                                      + ", Nome: " + nome[i]
                                      + ", Telefone: " + telefone[i]
                                      + ", Data de Pagamento: " + dataDePagamento[i].ToString("dd/MM/yyyy")
                                      + ", Valor Mensal: " + valorMensal[i]
                                      + ", CPF: " + CPF[i]
                                      + ", Endereço: " + endereco[i]
                                      + ", Modelo: " + daoVeiculo.modelo[i]
                                      + ", Fabricante: " + daoVeiculo.fabricante[i]
                                      + ", Cor: " + daoVeiculo.cor[i]
                                      + ", Placa: " + daoVeiculo.placa[i];

                    return msg;
                }

            }

            return "Código não encontrado!";
        }


        //Método para consultar todas as linhas
        public void ConsultarTudo()
        {
            //Preencher o Vetor
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                Console.WriteLine("\n\n Codigo:" + cod[i]
                                      + ",Nome: " + nome[i]
                                      + ",Telefone: " + telefone[i]
                                      + ",Data de Pagamento: " + dataDePagamento[i].ToString("dd/MM/yyyy")
                                      + ",Valor Mensal: " + valorMensal[i]
                                      + ",CPF: " + CPF[i]
                                      + ", Endereço: " + endereco[i]
                                      + ", Modelo: " + daoVeiculo.modelo[i]
                                      + ", Fabricante: " + daoVeiculo.fabricante[i]
                                      + ", Cor: " + daoVeiculo.cor[i]
                                      + ", Placa: " + daoVeiculo.placa[i]);
            }//fim do for

        }//fim do consultar tudo


        public void AtualizarData(string campo, DateTime data, int codigo)
        {
            try
            {
                resultado = $"update mensalidade set {campo} = '{data:yyyy-MM-dd}' where codigo = '{codigo}'";

                //Executar o script

                MySqlCommand sql = new MySqlCommand(resultado, conexao);
                resultado = "" + sql.ExecuteNonQuery();
                Console.WriteLine("Dado Atualizado com Sucesso!");

            }
            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!" + e);

            }
        }

        public void Atualizar(string campo, int codigo, string novoDado)
        {
            try
            {
                resultado = $"update mensalidade set {campo} = '{novoDado}' where codigo = '{codigo}'";

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
            resultado = $"delete from mensalidade where codigo = '{codigo}'";
            //Executar o Comando
            MySqlCommand sql = new MySqlCommand(resultado, conexao);
            resultado = "" + sql.ExecuteNonQuery();
            //Mensagem...
            Console.WriteLine("Dados excluidos com sucesso!");

        }//fim do deletar


    }//fim da Classe Cliente

}//fim do projeto
