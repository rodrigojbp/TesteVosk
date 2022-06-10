using System;
using System.IO;
using Vosk;

public class VoskDemo
{
    public const String DIRETORIO_MODEL = "model";
    public const String DIRETORIO_MODEL_SPK = "model-spk";
    public static String CAMINHO_ARQUIVO = @"sounds\test.wav";
    //public const String CAMINHO_ARQUIVO = @"sounds\1234.wav";

    public static void DemoBytes(Model model)
   {
        // Demo byte buffer
        VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);
        rec.SetMaxAlternatives(0);
        rec.SetWords(true);
        using(Stream source = File.OpenRead(CAMINHO_ARQUIVO)) {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0) {
                if (rec.AcceptWaveform(buffer, bytesRead)) {
                    Console.WriteLine(rec.Result());
                } else {
                    Console.WriteLine(rec.PartialResult());
                }
            }
        }
        Console.WriteLine(rec.FinalResult());
   }

   public static void DemoFloats(Model model)
   {
        // Demo float array
        VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);
        using(Stream source = File.OpenRead(CAMINHO_ARQUIVO)) {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0) {
                float[] fbuffer = new float[bytesRead / 2];
                for (int i = 0, n = 0; i < fbuffer.Length; i++, n+=2) {
                    fbuffer[i] = BitConverter.ToInt16(buffer, n);
                }
                if (rec.AcceptWaveform(fbuffer, fbuffer.Length)) {
                    Console.WriteLine(rec.Result());
                } else {
                    Console.WriteLine(rec.PartialResult());
                }
            }
        }
        Console.WriteLine(rec.FinalResult());
   }

   public static void DemoSpeaker(Model model)
   {
        // Output speakers
        SpkModel spkModel = new SpkModel(DIRETORIO_MODEL_SPK);
        VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);
        rec.SetSpkModel(spkModel);

        using(Stream source = File.OpenRead(CAMINHO_ARQUIVO)) {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0) {
                if (rec.AcceptWaveform(buffer, bytesRead)) {
                    Console.WriteLine(rec.Result());
                } else {
                    Console.WriteLine(rec.PartialResult());
                }
            }
        }
        Console.WriteLine(rec.FinalResult());
   }

   public static void Main()
   {
        // You can set to -1 to disable logging messages
        Vosk.Vosk.SetLogLevel(0);

        Model model_ingles = new Model(DIRETORIO_MODEL_SPK);
        DemoSpeaker(model_ingles);

        Model model = new Model(DIRETORIO_MODEL);
        //DemoBytes(model);
        //DemoFloats(model);
        DemoSpeaker(model);
        CAMINHO_ARQUIVO = "1234.wav";
        
    }
}
