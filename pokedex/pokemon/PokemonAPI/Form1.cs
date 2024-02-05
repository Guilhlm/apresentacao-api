using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PokemonAPI
{
    public partial class Pokedex : Form
    {
        private const string ApiBaseUrl = "https://pokeapi.co/api/v2/pokemon/"; // consulta da api, base de dados pokemon

        public Pokedex()
        {
            InitializeComponent(); // inicializaçao do console
        }

        private async void btnConsultaPokemon_Click(object sender, EventArgs e) // botao de consulta
        {
            string pokemonName = txtNomePokemon.Text.Trim(); // variavel recebendo o txt, e .trim remove os espaços brancos

            if (!string.IsNullOrEmpty(pokemonName))
            {
                try
                {
                    string apiUrl = $"{ApiBaseUrl}{pokemonName.ToLower()}/"; // chamando a api, variavel nome em minusculo
                    PokemonData pokemonData = await GetPokemonData(apiUrl); // api com variavel recebe funçao pokemon 

                    if (pokemonData != null) // se a variavel for diferente de vazio
                    {
                        DisplayPokemonInfo(pokemonData); // usa a api
                    }
                    else
                    {
                        MessageBox.Show("Pokémon não encontrado."); // se for vazio ele da erro
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao consultar a API: {ex.Message}"); // mesma coisa do if 
                }
            }
            else
            {
                MessageBox.Show("Digite o nome de um Pokémon.");
            }
        }

        private void DisplayPokemonInfo(PokemonData pokemonData) // escopo declarado, api e variavel
        {
            // Exibir informações do Pokémon
            lbID.Text = $"ID: {pokemonData.id}";
            lbName.Text = $"Nome: {pokemonData.name}";
            lbTipo.Text = $"Tipo: {string.Join(", ", pokemonData.types.Select(t => t.type.name))}";
            lbPeso.Text = $"Peso: {pokemonData.weight} kg";
            lbAltura.Text = $"Altura: {pokemonData.height} m";

            
            DisplayPokemonImage(pokemonData.sprites.front_default); // exibe a imagem do Pokémon
        }

        private async Task<PokemonData> GetPokemonData(string apiUrl)
        {
            using (HttpClient client = new HttpClient()) // usa o novo client gerado na variavel
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl); // espera o cliente consultanfo a api

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync(); // instalaçao atravez do nuget .jason
                    
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<PokemonData>(jsonResponse); //instale atravez do NuGet Newtonsoft.Json
                }
            }

            return null;
        }

        private void DisplayPokemonImage(string imageUrl) // imagem do pokemon
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                pictureBox1.Size = new Size(300, 300); // tamamho
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // tela full
                pictureBox1.Load(imageUrl); // carregar a imagem no console
            }
        }


        public class PokemonData // classe das informaçoes do pokemnon 
        {
            public string name { get; set; }
            public int id { get; set; }
            public List<TypeData> types { get; set; } // get recebe set seta a informaçao
            public float weight { get; set; }
            public float height { get; set; }
            public Sprites sprites { get; set; }
        }

        public class TypeData // classe do tipo pokemon
        {
            public Type type { get; set; } 
        }

        public class Type // classe do tipo nome
        {
            public string name { get; set; }
        }

        public class Sprites // classe do foto 
        {
            public string front_default { get; set; }
        }
    }

}
