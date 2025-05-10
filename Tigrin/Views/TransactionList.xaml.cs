using CommunityToolkit.Mvvm.Messaging;
using Tigrin.Litedb;
using Tigrin.Models;

namespace Tigrin.Views;

public partial class TransactionList : ContentPage
{
    private ITransactionRepository _repository;

    private void Reload()
    {
        var items = _repository.GetAll();
        ViewTransactions.ItemsSource = items;

        double income = items.Where(x => x.Type == Models.TransactionType.Income).Sum(x => x.Value);
        double expense = items.Where(x => x.Type == Models.TransactionType.Expense).Sum(x => x.Value);
        double balance = income - expense;

        LabelIncome.Text = income.ToString("C");
        LabelExpense.Text = expense.ToString("C");
        LabelBalance.Text = balance.ToString("C");
    }

    public TransactionList (ITransactionRepository repository)
	{

        this._repository = repository;
        InitializeComponent();
        Reload();

        //dependencia Nuget - CommunityToolkit.Mvvm
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            Reload();
        });

	}


	private void OnClickedToTransactionAdd(object sender, EventArgs e)
    {
        var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
		Navigation.PushModalAsync(transactionAdd);
    }

    private void TappedToEdit(object sender, TappedEventArgs e)
    {

        var grid = (Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        Transaction transaction = (Transaction)gesture.CommandParameter;

        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        transactionEdit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEdit);
    }

    private async void TappedToDelete(object sender, TappedEventArgs e)
    {
        bool result = await App.Current.MainPage.DisplayAlert("Remover", "Tem certeza que deseja remover esse item?", "Sim", "Não");
        if(result)
        {
            Transaction transaction = (Transaction)e.Parameter;
            _repository.Delete(transaction);
            Reload();
        }
    }
}