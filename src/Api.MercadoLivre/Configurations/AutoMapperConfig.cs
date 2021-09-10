using Api.MercadoLivre.Model;
using Api.MercadoLivre.Model.ViewModel.Caracteristica;
using Api.MercadoLivre.Model.ViewModel.Produto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Configurations
{
    public class AutoMapperConfig : Profile
    {

        public AutoMapperConfig()
        {
            CreateMap<Usuario, UsuarioAddViewModel>().ReverseMap();
            CreateMap<Usuario, UsuarioReturnViewModel>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginReturnViewModel>().ReverseMap();




            CreateMap<Categoria, CategoriaAddViewModel>().ReverseMap();
            CreateMap<Categoria, CategoriaAddReturnViewModel>().ReverseMap();
            CreateMap<Categoria, CategoriaDetalhesReturnViewModel>().ReverseMap();
            CreateMap<Categoria, CategoriaReturnViewModel>()
                .ForMember(parameter => parameter.CategoriaPrincipalNome, map => map.MapFrom(o => o.CategoriaPrincipal.Nome));





            CreateMap<CaracteristicaAddViewModel, CaracteristicaBindViewModel>();
            CreateMap<CaracteristicaBindViewModel, Caracteristica>().ReverseMap();
            CreateMap<Caracteristica, CaracteristicaReturnViewModel>();




            CreateMap<Produto, ProdutoReturnViewModel>()
                .ForMember(parameter => parameter.CategoriaNome, map => map.MapFrom(o => o.Categoria.Nome));
            CreateMap<Produto, ProdutoReturnSimpleViewModel>()
                .ForMember(parameter => parameter.CategoriaNome, map => map.MapFrom(o => o.Categoria.Nome))
                .ForMember(parameter => parameter.Caracteristicas, map => map.MapFrom(o => o.Caracteristicas.Count));
            CreateMap<Produto, ProdutoAddReturnViewModel>()
                .ForMember(parameter => parameter.Caracteristicas, opt => opt.Ignore())
                .ForMember(parameter => parameter.CategoriaNome, opt => opt.Ignore());
            CreateMap<ProdutoAddViewModel, Produto>()
                .ForMember(parameter => parameter.Caracteristicas, opt => opt.Ignore());



            CreateMap<OpiniaoAddViewModel, Opiniao>();
            CreateMap<Opiniao, OpiniaoAddReturnViewModel>();
            CreateMap<Opiniao, OpiniaoReturnViewModel>();




            CreateMap<PerguntaAddViewModel, Pergunta>();
            CreateMap<Pergunta, PerguntaReturnViewModel>();
            CreateMap<Pergunta, PerguntaAddReturnViewModel>();




            CreateMap<Imagem, ImagemAddReturnViewModel>().ReverseMap();




            CreateMap<CompraAddViewModel, Compra>();
            CreateMap<Compra, CompraReturnViewModel>();
            CreateMap<Produto, ProdutoCompraReturnViewModel>();
            CreateMap<Compra, CompraReturnViewModel>()
                .ForMember(parameter => parameter.Comprador, map => map.MapFrom(o => o.Usuario));



            CreateMap<PagamentoAddViewModel, Pagamento>();
            CreateMap<Pagamento, PagamentoReturnViewModel>();

        }

    }
}
