using MongoDB.Bson;
using MongoDB.Driver;
using System;

class Program {
    static void Main(string[] args) {
        var connectionString = "mongodb://admin:123456@localhost:27017";
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("Filmes");
        var collection = database.GetCollection<BsonDocument>("Filmes");

        Console.WriteLine("Escolha uma opção:");
        Console.WriteLine("1 - Adicionar novo filme");
        Console.WriteLine("2 - Mostrar todos os filmes");
        Console.WriteLine("3 - Pesquisar por ID");
        Console.WriteLine("4 - Deletar por ID");

        var opcao = Console.ReadLine();

        switch (opcao) {
            case "1":
                AdicionarFilme(collection);
                break;
            case "2":
                MostrarFilmes(collection);
                break;
            case "3":
                PesquisarPorId(collection);
                break;
            case "4":
                DeletarPorId(collection);
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    static void AdicionarFilme(IMongoCollection<BsonDocument> collection) {
        Console.Write("Digite o título do filme: ");
        var titulo = Console.ReadLine();

        Console.Write("Digite o ano do filme: ");
        if (!int.TryParse(Console.ReadLine(), out int ano)) {
            Console.WriteLine("Ano inválido.");
            return;
        }

        Console.Write("Digite o gênero do filme: ");
        var genero = Console.ReadLine();

        var filmeParaAdicionar = new BsonDocument
        {
            { "titulo", titulo },
            { "ano", ano },
            { "genero", genero }
            // Adicione outros campos conforme necessário
        };

        collection.InsertOne(filmeParaAdicionar);
        Console.WriteLine("Filme adicionado com sucesso.");
    }

    static void MostrarFilmes(IMongoCollection<BsonDocument> collection) {
        var documentos = collection.Find(new BsonDocument()).ToList();

        if (documentos.Count == 0) {
            Console.WriteLine("Nenhum filme encontrado.");
        } else {
            foreach (var documento in documentos) {
                Console.WriteLine("Documento encontrado: " + documento);
            }
        }
    }

    static void PesquisarPorId(IMongoCollection<BsonDocument> collection) {
        Console.Write("Digite o ID do filme: ");
        var id = Console.ReadLine();

        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
        var resultado = collection.Find(filter).FirstOrDefault();

        if (resultado == null) {
            Console.WriteLine("Nenhum filme encontrado com o ID fornecido.");
        } else {
            Console.WriteLine("Filme encontrado: " + resultado);
        }
    }

    static void DeletarPorId(IMongoCollection<BsonDocument> collection) {
        Console.Write("Digite o ID do filme que deseja deletar: ");
        var id = Console.ReadLine();

        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
        var resultado = collection.Find(filter).FirstOrDefault();

        if (resultado == null) {
            Console.WriteLine("Nenhum filme encontrado com o ID fornecido.");
        } else {
            collection.DeleteOne(filter);
            Console.WriteLine("Filme deletado com sucesso.");
        }
    }
}
