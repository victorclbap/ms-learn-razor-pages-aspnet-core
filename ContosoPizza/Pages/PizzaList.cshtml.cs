using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Pages
{
    public class PizzaListModel : PageModel
    {
        // A palavra-chave readonly indica que o valor da variável _service não pode ser alterado após ser definido no construtor.
        // PizzaList é inicializado para default! a fim de indicar ao compilador que ele será inicializado posteriormente, portanto, verificações de segurança nulas não são necessárias.
        private readonly PizzaService _service;
        public IList<Pizza> PizzaList {get;set;} = default!;

        // O atributo BindProperty é usado para associar a propriedade NewPizza à página Razor. Quando uma solicitação HTTP POST é feita,
        // a propriedade NewPizza é preenchida com a entrada de usuário.
        // A palavra-chave default! é usada para inicializar a propriedade NewPizza para null. Isso impede que o compilador gere um aviso sobre a propriedade NewPizza não estar inicializada.

        [BindProperty]
        public Pizza NewPizza { get; set; } = default!;


        public PizzaListModel(PizzaService service){
            _service = service;
        }

        public void OnGet(){
            PizzaList = _service.GetPizzas();
        }


        // A propriedade ModelState.IsValid é usada para determinar se a entrada do usuário é válida.
        // As regras de validação são inferidas de atributos (como Required e Range) na classe Pizza emModels\Pizza.cs.
        // Se a entrada do usuário for inválida, o método Page será chamado para renderizar a página novamente.
        public IActionResult OnPost(){
            if(!ModelState.IsValid || NewPizza == null){
                return Page();
            }

            _service.AddPizza(NewPizza);
            return RedirectToAction("Get");
            
        }


        // A página sabe que deve usar esse método porque o atributo asp-page-handler no botão Excluir em Pages\PizzaList.cshtml está definido como Delete.
        // O parâmetro id está associado ao valor da rota id na URL. Isso é feito com o atributo asp-route-id no botão Excluir em Pages\PizzaList.cshtml.
        public IActionResult OnPostDelete(int id){
            _service.DeletePizza(id);
            return RedirectToAction("Get");
        }

    }
}
