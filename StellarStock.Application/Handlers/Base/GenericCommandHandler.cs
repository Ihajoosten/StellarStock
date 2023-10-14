namespace StellarStock.Application.Handlers.Base
{
    public class GenericCommandHandler<TCommand, TEntity> : IGenericCommandHandler<TCommand, TEntity> where TCommand : ICommand where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;

        public GenericCommandHandler(IGenericRepository<TEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task HandleAsync(TCommand command, TEntity entity)
        {
            switch (entity)
            {
                case InventoryItem:
                    switch (command)
                    {
                        // Inventory Item Commands
                        case CreateInventoryItemCommand createInventoryItemCommand:
                            await HandleCreateInventoryItemAsync(createInventoryItemCommand);
                            break;
                        case UpdateInventoryItemCommand updateInventoryItemCommand:
                            await HandleUpdateInventoryItemAsync(updateInventoryItemCommand);
                            break;
                        case DeleteInventoryItemCommand deleteInventoryItemCommand:
                            await HandleDeleteInventoryItemAsync(deleteInventoryItemCommand);
                            break;
                        default: throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
                    };
                    break;
                case Supplier:
                    switch (command)
                    {
                        // Supplier Commands
                        case CreateSupplierCommand createSupplierCommand:
                            await HandleCreateSupplierAsync(createSupplierCommand);
                            break;
                        case UpdateSupplierCommand updateSupplierCommand:
                            await HandleUpdateSupplierAsync(updateSupplierCommand);
                            break;
                        case DeleteSupplierCommand deleteSupplierCommand:
                            await HandleDeleteSupplierAsync(deleteSupplierCommand);
                            break;
                        case ActivateSupplierCommand activateSupplierCommand:
                            await HandleActivateSupplierAsync(activateSupplierCommand);
                            break;
                        case DeactivateSupplierCommand deactivateSupplierCommand:
                            await HandleDeactivateSupplierAsync(deactivateSupplierCommand);
                            break;
                    };
                    break;
                case Warehouse:
                    switch (command)
                    {
                        // Warehouse Commands
                        case CreateWarehouseCommand createWarehouseCommand:
                            await HandleCreateWarehouseAsync(createWarehouseCommand);
                            break;
                        case UpdateWarehouseCommand updateWarehouseCommand:
                            await HandleUpdateWarehouseAsync(updateWarehouseCommand);
                            break;
                        case DeleteWarehouseCommand deleteWarehouseCommand:
                            await HandleDeleteWarehouseAsync(deleteWarehouseCommand);
                            break;
                        case CloseWarehouseCommand closeWarehouseCommand:
                            await HandleCloseWarehouseAsync(closeWarehouseCommand);
                            break;
                        case ReopenWarehouseCommand reopenWarehouseCommand:
                            await HandleReopenWarehouseAsync(reopenWarehouseCommand);
                            break;
                        case MoveWarehouseCommand moveWarehouseCommand:
                            await HandleMoveWarehouseAsync(moveWarehouseCommand);
                            break;
                    };
                    break;
                default: throw new ArgumentException($"Unsupported Entity type: {typeof(TEntity)}");
            }
        }

        // Inventory Item handlers
        private async Task HandleCreateInventoryItemAsync(CreateInventoryItemCommand createCommand)
        {
            var item = new InventoryItem
            {
                Id = new Guid().ToString(),
                Name = createCommand.Name,
                Description = createCommand.Description,
                Category = createCommand.Category,
                PopularityScore = createCommand.PopularityScore,
                ProductCode = createCommand.ProductCode,
                Quantity = createCommand.Quantity,
                Money = createCommand.Money,
                ValidityPeriod = createCommand.ValidityPeriod,
                WarehouseId = createCommand.WarehouseId,
                SupplierId = createCommand.SupplierId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _repository.AddAsync(item as TEntity);
        }
        private async Task HandleUpdateInventoryItemAsync(UpdateInventoryItemCommand updateCommand)
        {
            var item = await _repository.GetByIdAsync(updateCommand.InventoryItemId) as InventoryItem;
            if (item != null)
            {
                item.Name = updateCommand.NewName;
                item.Description = updateCommand.NewDescription;
                item.Category = updateCommand.NewCategory;
                item.ProductCode = updateCommand.NewProductCode;
                item.Quantity = updateCommand.NewQuantity;
                item.Money = updateCommand.NewMoney;
                item.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(item as TEntity);
            }
        }
        private async Task HandleDeleteInventoryItemAsync(DeleteInventoryItemCommand deleteCommand)
        {
            var item = await _repository.GetByIdAsync(deleteCommand.Id) as InventoryItem;
            if (item != null)
            {
                await _repository.RemoveAsync(item.Id);
            }
        }

        // Supplier handlers
        private async Task HandleCreateSupplierAsync(CreateSupplierCommand createSupplierCommand)
        {
            var supplier = new Supplier
            {
                Id = Guid.NewGuid().ToString(),
                Name = createSupplierCommand.Name,
                Phone = createSupplierCommand.Phone,
                ContactEmail = createSupplierCommand.ContactEmail,
                Address = createSupplierCommand.Address,
                IsActive = true,
                ValidityPeriod = createSupplierCommand.ValidityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _repository.AddAsync(supplier as TEntity);
        }
        private async Task HandleUpdateSupplierAsync(UpdateSupplierCommand updateSupplierCommand)
        {
            var supplier = await _repository.GetByIdAsync(updateSupplierCommand.SupplierId) as Supplier;
            if (supplier != null)
            {
                supplier.Name = updateSupplierCommand.NewName;
                supplier.Phone = updateSupplierCommand.NewPhone;
                supplier.ContactEmail = updateSupplierCommand.NewContactEmail;
                supplier.Address = updateSupplierCommand.NewAddress;
                supplier.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(supplier as TEntity);
            }
        }
        private async Task HandleDeleteSupplierAsync(DeleteSupplierCommand deleteSupplierCommand)
        {
            var supplier = await _repository.GetByIdAsync(deleteSupplierCommand.Id) as Supplier;
            if (supplier != null)
            {
                await _repository.RemoveAsync(supplier.Id);
            }
        }
        private async Task HandleActivateSupplierAsync(ActivateSupplierCommand deactivateSupplierCommand)
        {
            var supplier = await _repository.GetByIdAsync(deactivateSupplierCommand.SupplierId) as Supplier;
            if (supplier != null)
            {
                supplier.IsActive = true;
                supplier.UpdatedAt = DateTime.Now;

                await _repository.UpdateAsync(supplier as TEntity);
            }
        }
        private async Task HandleDeactivateSupplierAsync(DeactivateSupplierCommand deactivateSupplierCommand)
        {
            var supplier = await _repository.GetByIdAsync(deactivateSupplierCommand.SupplierId) as Supplier;
            if (supplier != null)
            {
                supplier.IsActive = false;
                supplier.UpdatedAt = DateTime.Now;

                await _repository.UpdateAsync(supplier as TEntity);
            }
        }

        // Warehouse handlers
        private async Task HandleCreateWarehouseAsync(CreateWarehouseCommand createWarehouseCommand)
        {
            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid().ToString(),
                Name = createWarehouseCommand.Name,
                Phone = createWarehouseCommand.Phone,
                Address = createWarehouseCommand.Address,
                IsOpen = createWarehouseCommand.IsOpen,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _repository.AddAsync(warehouse as TEntity);
        }
        private async Task HandleUpdateWarehouseAsync(UpdateWarehouseCommand updateWarehouseCommand)
        {
            var warehouse = await _repository.GetByIdAsync(updateWarehouseCommand.WarehouseId) as Warehouse;
            if (warehouse != null)
            {
                warehouse.Name = updateWarehouseCommand.NewName;
                warehouse.Phone = updateWarehouseCommand.NewPhone;
                warehouse.Address = updateWarehouseCommand.NewAddress;
                warehouse.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(warehouse as TEntity);
            }
        }
        private async Task HandleDeleteWarehouseAsync(DeleteWarehouseCommand deleteWarehouseCommand)
        {
            var warehouse = await _repository.GetByIdAsync(deleteWarehouseCommand.WarehouseId) as Warehouse;
            if (warehouse != null)
            {
                await _repository.RemoveAsync(warehouse.Id);
            }
        }
        private async Task HandleCloseWarehouseAsync(CloseWarehouseCommand closeWarehouseCommand)
        {
            var warehouse = await _repository.GetByIdAsync(closeWarehouseCommand.WarehouseId) as Warehouse;
            if (warehouse != null)
            {
                warehouse.IsOpen = false;
                warehouse.UpdatedAt = DateTime.Now;

                await _repository.UpdateAsync(warehouse as TEntity);
            }
        }
        private async Task HandleReopenWarehouseAsync(ReopenWarehouseCommand reopenWarehouseCommand)
        {
            var warehouse = await _repository.GetByIdAsync(reopenWarehouseCommand.WarehouseId) as Warehouse;
            if (warehouse != null)
            {
                warehouse.IsOpen = true;
                warehouse.UpdatedAt = DateTime.Now;

                await _repository.UpdateAsync(warehouse as TEntity);
            }
        }
        private async Task HandleMoveWarehouseAsync(MoveWarehouseCommand moveWarehouseCommand)
        {
            var warehouse = await _repository.GetByIdAsync(moveWarehouseCommand.WarehouseId) as Warehouse;
            if (warehouse != null)
            {
                warehouse.Address.Street = moveWarehouseCommand.NewAddress;
                warehouse.Address.Region = moveWarehouseCommand.NewRegion;
                warehouse.Address.City = moveWarehouseCommand.NewCity;
                warehouse.Address.Country = moveWarehouseCommand.NewCountry;
                warehouse.Address.PostalCode = moveWarehouseCommand.NewPostalCode;

                warehouse.IsOpen = true;
                warehouse.UpdatedAt = DateTime.Now;

                await _repository.UpdateAsync(warehouse as TEntity);
            }
        }
    }
}