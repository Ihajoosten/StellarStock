namespace StellarStock.Application.Handlers.Base
{
    public class GenericCommandHandler<TCommand> : IGenericCommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task HandleAsync(TCommand command)
        {
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
                default: throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
            };
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

            await _unitOfWork.GetRepository<InventoryItem>()!.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task HandleUpdateInventoryItemAsync(UpdateInventoryItemCommand updateCommand)
        {
            var item = await _unitOfWork.GetRepository<InventoryItem>()!.GetByIdAsync(updateCommand.InventoryItemId);
            if (item != null)
            {
                item.Name = updateCommand.NewName;
                item.Description = updateCommand.NewDescription;
                item.Category = updateCommand.NewCategory;
                item.ProductCode = updateCommand.NewProductCode;
                item.Quantity = updateCommand.NewQuantity;
                item.Money = updateCommand.NewMoney;
                item.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.GetRepository<InventoryItem>()!.UpdateAsync(item);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task HandleDeleteInventoryItemAsync(DeleteInventoryItemCommand deleteCommand)
        {
            var item = await _unitOfWork.GetRepository<InventoryItem>()!.GetByIdAsync(deleteCommand.Id);
            if (item != null)
            {
                await _unitOfWork.GetRepository<InventoryItem>()!.RemoveAsync(item.Id);
                await _unitOfWork.SaveChangesAsync();
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

            await _unitOfWork.GetRepository<Supplier>()!.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task HandleUpdateSupplierAsync(UpdateSupplierCommand updateSupplierCommand)
        {
            var supplier = await _unitOfWork.GetRepository<Supplier>()!.GetByIdAsync(updateSupplierCommand.SupplierId);
            if (supplier != null)
            {
                supplier.Name = updateSupplierCommand.NewName;
                supplier.Phone = updateSupplierCommand.NewPhone;
                supplier.ContactEmail = updateSupplierCommand.NewContactEmail;
                supplier.Address = updateSupplierCommand.NewAddress;
                supplier.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.GetRepository<Supplier>()!.UpdateAsync(supplier);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task HandleDeleteSupplierAsync(DeleteSupplierCommand deleteSupplierCommand)
        {
            var supplier = await _unitOfWork.GetRepository<Supplier>()!.GetByIdAsync(deleteSupplierCommand.Id);
            if (supplier != null)
            {
                await _unitOfWork.GetRepository<Supplier>()!.RemoveAsync(supplier.Id);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task HandleActivateSupplierAsync(ActivateSupplierCommand activateSupplierCommand)
        {
            var supplier = await _unitOfWork.GetRepository<Supplier>()!.GetByIdAsync(activateSupplierCommand.SupplierId);
            if (supplier != null)
            {
                supplier.IsActive = true;
                supplier.UpdatedAt = DateTime.Now;

                await _unitOfWork.GetRepository<Supplier>()!.UpdateAsync(supplier);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task HandleDeactivateSupplierAsync(DeactivateSupplierCommand deactivateSupplierCommand)
        {
            var supplier = await _unitOfWork.GetRepository<Supplier>()!.GetByIdAsync(deactivateSupplierCommand.SupplierId);
            if (supplier != null)
            {
                supplier.IsActive = false;
                supplier.UpdatedAt = DateTime.Now;

                await _unitOfWork.GetRepository<Supplier>()!.UpdateAsync(supplier);
                await _unitOfWork.SaveChangesAsync();
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

            await _unitOfWork.GetRepository<Warehouse>()!.AddAsync(warehouse);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task HandleUpdateWarehouseAsync(UpdateWarehouseCommand updateWarehouseCommand)
        {
            var warehouse = await _unitOfWork.GetRepository<Warehouse>()!.GetByIdAsync(updateWarehouseCommand.WarehouseId);
            if (warehouse != null)
            {
                warehouse.Name = updateWarehouseCommand.NewName;
                warehouse.Phone = updateWarehouseCommand.NewPhone;
                warehouse.Address = updateWarehouseCommand.NewAddress;
                warehouse.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.GetRepository<Warehouse>()!.UpdateAsync(warehouse);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task HandleDeleteWarehouseAsync(DeleteWarehouseCommand deleteWarehouseCommand)
        {
            var warehouse = await _unitOfWork.GetRepository<Warehouse>()!.GetByIdAsync(deleteWarehouseCommand.WarehouseId);
            if (warehouse != null)
            {
                await _unitOfWork.GetRepository<Warehouse>()!.RemoveAsync(warehouse.Id);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task HandleCloseWarehouseAsync(CloseWarehouseCommand closeWarehouseCommand)
        {
            var warehouse = await _unitOfWork.GetRepository<Warehouse>()!.GetByIdAsync(closeWarehouseCommand.WarehouseId);
            if (warehouse != null)
            {
                warehouse.IsOpen = false;
                warehouse.UpdatedAt = DateTime.Now;

                await _unitOfWork.GetRepository<Warehouse>()!.UpdateAsync(warehouse);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task HandleReopenWarehouseAsync(ReopenWarehouseCommand reopenWarehouseCommand)
        {
            var warehouse = await _unitOfWork.GetRepository<Warehouse>()!.GetByIdAsync(reopenWarehouseCommand.WarehouseId);
            if (warehouse != null)
            {
                warehouse.IsOpen = true;
                warehouse.UpdatedAt = DateTime.Now;

                await _unitOfWork.GetRepository<Warehouse>()!.UpdateAsync(warehouse);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        private async Task HandleMoveWarehouseAsync(MoveWarehouseCommand moveWarehouseCommand)
        {
            var warehouse = await _unitOfWork.GetRepository<Warehouse>()!.GetByIdAsync(moveWarehouseCommand.WarehouseId);
            if (warehouse != null)
            {
                warehouse.Address.Street = moveWarehouseCommand.NewAddress;
                warehouse.Address.Region = moveWarehouseCommand.NewRegion;
                warehouse.Address.City = moveWarehouseCommand.NewCity;
                warehouse.Address.Country = moveWarehouseCommand.NewCountry;
                warehouse.Address.PostalCode = moveWarehouseCommand.NewPostalCode;

                warehouse.IsOpen = true;
                warehouse.UpdatedAt = DateTime.Now;

                await _unitOfWork.GetRepository<Warehouse>()!.UpdateAsync(warehouse);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
