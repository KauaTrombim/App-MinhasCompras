using MinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private ObservableCollection<Produto> listaCompleta = new ObservableCollection<Produto>();
    public ObservableCollection<Produto> listaFiltrada { get; set; } = new ObservableCollection<Produto>();

    public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = listaFiltrada;
	}

    protected async override void OnAppearing()
	{
        //evitando as duplicações
        listaCompleta.Clear();
        listaFiltrada.Clear();

        List<Produto> tmp = await App.DB.GetAll();

        foreach (var item in tmp)
        {
            listaCompleta.Add(item);
            listaFiltrada.Add(item);
        }

    }	

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		string q = e.NewTextValue;

		listaFiltrada.Clear();

        List<Produto> tmp = await App.DB.Search(q);

        foreach (var produto in listaCompleta)
        {
            if (produto.Descricao.ToLower().Contains(q))
            {
                listaFiltrada.Add(produto);
            }
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = listaFiltrada.Sum(i => i.Total);

		string msg = $"O total é: {soma:C}";

		DisplayAlert("Total dos produtos", msg, "OK");
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        //ainda será implementado o remove
    }
}