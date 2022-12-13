namespace KeyManager.Domain.Tests.Mappings;

public class AutoMapperProfileTests
{
    [Fact]
    public Task Given_DomainAutoMapper_When_ValidateMappings_Then_ShouldBeValid()
    {
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
        IMapper mapper = new Mapper(mapperConfig);
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        return Task.CompletedTask;
    }
}