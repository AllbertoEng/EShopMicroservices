namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name,
                                       List<string> Category,
                                       string Description,
                                       string ImageFile,
                                       decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(p => p.Category).NotEmpty().WithMessage("Product category is required.");
            RuleFor(p => p.ImageFile).NotEmpty().WithMessage("Product description is required.");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Product price must be greater than 0.");
        }
    }

    internal class CreateProductCommandHandler
        (IDocumentSession session, ILogger<CreateProductCommandHandler> logger) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //business logic to create a product:
            logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);

            //Create a product entity from command object      
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description =  command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price 
            };

            //Save the product entity to the database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            //return result (CreateProductResult) 
            return new CreateProductResult(product.Id);
        }
    }
}
