using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.Address.Contracts;

[Mapper]
public static partial class AddressMapper
{
    public static partial Models.Address ToAddress(AddressRequest addressRequest);
}
