 using System;
 using System.Collections.Generic;
 using System.Drawing;
 using System.IO;
 using System.Linq;
 using System.Net.Http;
 using System.Threading;
 using System.Threading.Tasks;

namespace didaticos.redimensionador
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando redimensionador");

            Thread thread = new Thread(Redimensionar);
            thread.Start();







            Console.Read();
        }

        static void Redimensionar()
        {
            #region "diretorios"
            string diretorio_Entrada = "Arquivos_Entrada";
            string diretorio_Redimensionados = "Arquivos_Redimensionados";
            string diretorio_Finalizados = "Arquivos_Finalizados";


            if (!Directory.Exists(diretorio_Entrada))
            {
                Directory.CreateDirectory(diretorio_Entrada);
            }


            if (!Directory.Exists(diretorio_Redimensionados))
            {
                Directory.CreateDirectory(diretorio_Redimensionados);
            }


            if (!Directory.Exists(diretorio_Finalizados))
            {
                Directory.CreateDirectory(diretorio_Finalizados);
            }
            #endregion


            FileStream fileStream;
            FileInfo fileinfo;
            while (true)
            {
                //Meu programa vai olhar para a pasta de entrada
                //Se tiver arquivos, ele vai redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorio_Entrada);

                //ler o tamanho que irá redimensionar
                int novaAltura = 200;

                foreach (var arquivo in arquivosEntrada)
                {
                     fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                     fileinfo = new FileInfo(arquivo);




                    string caminho = Environment.CurrentDirectory + @"\" + diretorio_Redimensionados + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fileinfo.Name;



                    //Redimensiona + // copia o arquivo redimensionado para a pasta de redimensionados
                    Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);

                    //Fecha o arquivo para liberar o acesso a memoria
                    fileStream.Close();

                    //Moveo arquivo entrada para a pasta de finalizados
                    String caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_Finalizados + @"\" + fileinfo.Name;
                    //Copia os arquivo redimensionado para a pasta de redimensionados
                    fileinfo.MoveTo(caminhoFinalizado);

                }

                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image">Imagem a ser redimensionada</param>
        /// <param name="altura">Altura que desejamos redimensionar</param>
        /// <param name="caminho">Caminho aonde iremos gravar o arquivo redimensionado</param>
        /// <returns></returns>
        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }

            novaImage.Save(caminho);
            imagem.Dispose();
        }
    }


}
