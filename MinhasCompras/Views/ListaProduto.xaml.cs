using MinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
        try
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
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
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
        try
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
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }


    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double soma = listaFiltrada.Sum(i => i.Total);

            string msg = $"O total é: {soma:C}";

            DisplayAlert("Total dos produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }


    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

    }

    private async Task MenuItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert("Tem certeza que deseja Excluir?", "Remover Produto?", "Sim", "Não");

            if (confirm)
            {
                await App.DB.Delete(p.Id);
                listaFiltrada.Remove(p);
                listaCompleta.Remove(p);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}