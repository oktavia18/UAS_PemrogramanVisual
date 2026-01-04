using MongoDB.Driver;
using Microsoft.Extensions.Options;
using laundrycucikilat.Models;

namespace laundrycucikilat.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Order> _ordersCollection;
        private readonly IMongoCollection<ContactMessage> _contactMessagesCollection;
        private readonly IMongoCollection<Employee> _employeesCollection;
        private readonly IMongoCollection<Service> _servicesCollection;
        private readonly IMongoCollection<Customer> _customersCollection;

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);

            _ordersCollection = _database.GetCollection<Order>(mongoDbSettings.Value.OrdersCollectionName);
            _contactMessagesCollection = _database.GetCollection<ContactMessage>(mongoDbSettings.Value.ContactMessagesCollectionName);
            _employeesCollection = _database.GetCollection<Employee>("employees");
            _servicesCollection = _database.GetCollection<Service>("services");
            _customersCollection = _database.GetCollection<Customer>("customers");
        }

        // Generic method to get any collection
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        // Order Methods
        public async Task<List<Order>> GetOrdersAsync() =>
            await _ordersCollection.Find(_ => true).ToListAsync();

        public async Task<Order?> GetOrderAsync(string orderId) =>
            await _ordersCollection.Find(x => x.OrderId == orderId).FirstOrDefaultAsync();

        public async Task<Order?> GetOrderByIdAsync(string id) =>
            await _ordersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateOrderAsync(Order newOrder)
        {
            newOrder.CreatedAt = DateTime.UtcNow;
            newOrder.UpdatedAt = DateTime.UtcNow;
            await _ordersCollection.InsertOneAsync(newOrder);
        }

        public async Task UpdateOrderAsync(string id, Order updatedOrder)
        {
            updatedOrder.UpdatedAt = DateTime.UtcNow;
            await _ordersCollection.ReplaceOneAsync(x => x.Id == id, updatedOrder);
        }

        public async Task<bool> UpdateOrderStatusAsync(string orderId, string status)
        {
            var update = Builders<Order>.Update
                .Set(x => x.Status, status)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);
            
            var result = await _ordersCollection.UpdateOneAsync(x => x.OrderId == orderId, update);
            return result.ModifiedCount > 0;
        }

        public async Task UpdateOrderPaymentAsync(string orderId, string paymentMethod, string paymentStatus, string paymentDate)
        {
            var update = Builders<Order>.Update
                .Set(x => x.PaymentMethod, paymentMethod)
                .Set(x => x.PaymentStatus, paymentStatus)
                .Set(x => x.PaymentDate, paymentDate)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);
            
            await _ordersCollection.UpdateOneAsync(x => x.OrderId == orderId, update);
        }

        public async Task RemoveOrderAsync(string id) =>
            await _ordersCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Order>> GetRecentOrdersAsync(int limit = 10) =>
            await _ordersCollection.Find(_ => true)
                .SortByDescending(x => x.CreatedAt)
                .Limit(limit)
                .ToListAsync();

        // Contact Message Methods
        public async Task<List<ContactMessage>> GetContactMessagesAsync() =>
            await _contactMessagesCollection.Find(_ => true)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

        public async Task<ContactMessage?> GetContactMessageAsync(string id) =>
            await _contactMessagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateContactMessageAsync(ContactMessage newMessage)
        {
            newMessage.CreatedAt = DateTime.UtcNow;
            await _contactMessagesCollection.InsertOneAsync(newMessage);
        }

        public async Task MarkMessageAsReadAsync(string id)
        {
            var update = Builders<ContactMessage>.Update.Set(x => x.IsRead, true);
            await _contactMessagesCollection.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task RemoveContactMessageAsync(string id) =>
            await _contactMessagesCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<long> GetUnreadMessagesCountAsync() =>
            await _contactMessagesCollection.CountDocumentsAsync(x => !x.IsRead);

        // Employee Methods
        public async Task<List<Employee>> GetEmployeesAsync() =>
            await _employeesCollection.Find(_ => true).ToListAsync();

        public async Task<Employee?> GetEmployeeAsync(string employeeId) =>
            await _employeesCollection.Find(x => x.EmployeeId == employeeId).FirstOrDefaultAsync();

        public async Task<Employee?> GetEmployeeByIdAsync(string id) =>
            await _employeesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateEmployeeAsync(Employee newEmployee)
        {
            newEmployee.CreatedAt = DateTime.UtcNow;
            newEmployee.UpdatedAt = DateTime.UtcNow;
            await _employeesCollection.InsertOneAsync(newEmployee);
        }

        public async Task<bool> UpdateEmployeeAsync(string employeeId, Employee updatedEmployee)
        {
            updatedEmployee.UpdatedAt = DateTime.UtcNow;
            var result = await _employeesCollection.ReplaceOneAsync(x => x.EmployeeId == employeeId, updatedEmployee);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeId)
        {
            var result = await _employeesCollection.DeleteOneAsync(x => x.EmployeeId == employeeId);
            return result.DeletedCount > 0;
        }

        // Service Methods
        public async Task<List<Service>> GetServicesAsync() =>
            await _servicesCollection.Find(_ => true).ToListAsync();

        public async Task<Service?> GetServiceAsync(string serviceId) =>
            await _servicesCollection.Find(x => x.ServiceId == serviceId).FirstOrDefaultAsync();

        public async Task<Service?> GetServiceByIdAsync(string id) =>
            await _servicesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateServiceAsync(Service newService)
        {
            newService.CreatedAt = DateTime.UtcNow;
            newService.UpdatedAt = DateTime.UtcNow;
            await _servicesCollection.InsertOneAsync(newService);
        }

        public async Task<bool> UpdateServiceAsync(string serviceId, Service updatedService)
        {
            updatedService.UpdatedAt = DateTime.UtcNow;
            var result = await _servicesCollection.ReplaceOneAsync(x => x.ServiceId == serviceId, updatedService);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteServiceAsync(string serviceId)
        {
            var result = await _servicesCollection.DeleteOneAsync(x => x.ServiceId == serviceId);
            return result.DeletedCount > 0;
        }

        // Customer Methods
        public async Task<List<Customer>> GetCustomersAsync() =>
            await _customersCollection.Find(_ => true).ToListAsync();

        public async Task<Customer?> GetCustomerAsync(string customerId) =>
            await _customersCollection.Find(x => x.CustomerId == customerId).FirstOrDefaultAsync();

        public async Task<Customer?> GetCustomerByIdAsync(string id) =>
            await _customersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Customer?> GetCustomerByPhoneAsync(string noTelepon) =>
            await _customersCollection.Find(x => x.NoTelepon == noTelepon).FirstOrDefaultAsync();

        public async Task CreateCustomerAsync(Customer newCustomer)
        {
            newCustomer.CreatedAt = DateTime.UtcNow;
            newCustomer.UpdatedAt = DateTime.UtcNow;
            await _customersCollection.InsertOneAsync(newCustomer);
        }

        public async Task<bool> UpdateCustomerAsync(string customerId, Customer updatedCustomer)
        {
            updatedCustomer.UpdatedAt = DateTime.UtcNow;
            var result = await _customersCollection.ReplaceOneAsync(x => x.CustomerId == customerId, updatedCustomer);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteCustomerAsync(string customerId)
        {
            var result = await _customersCollection.DeleteOneAsync(x => x.CustomerId == customerId);
            return result.DeletedCount > 0;
        }
    }
}