using APP.Models;
using APP.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Controllers
{
    public class UserController : Controller
    {
        private DB dataBase = new DB();
        public string Index()
        {
            return "Users";
        }

        // Get all clients
        public List<Client> Clients()
        {
            List<Client> clientList = new List<Client>();
            DataTable dataTable = dataBase.getDatas("select * from client");

            clientList = (from DataRow dataRow in dataTable.Rows
                          select new Client()
                          {
                              id = Convert.ToInt32(dataRow["id"]),
                              name = dataRow["name"].ToString(),
                              lastname = dataRow["last_name"].ToString(),
                              document = dataRow["bill_id"].ToString(),
                          }).ToList();

            return clientList;
        }

        // Get client
        public List<Client> Client(int id)
        {
            List<Client> clientList = new List<Client>();
            DataTable dataTable = dataBase.getDatas($"select * from client where id = {id}");

            clientList = (from DataRow dataRow in dataTable.Rows
                          select new Client()
                          {
                              id = Convert.ToInt32(dataRow["id"]),
                              name = dataRow["name"].ToString(),
                              lastname = dataRow["last_name"].ToString(),
                              document = dataRow["bill_id"].ToString(),
                          }).ToList();

            return clientList;
        }

        // Get invoice and inoice detaill
        public List<Invoice> Invoice(int id)
        {
            List<InvoiceDetaill> invoiceDetaill = new List<InvoiceDetaill>();
            List<Invoice> invoiceList = new List<Invoice>();

            DataTable dataTableInvoice = dataBase.getDatas($"select * from invoice where id = {id}");
            DataTable dataTableDetaillInvoice = dataBase.getDatas($"select * from invoicedetaill where client_bill_id = {id}");

            invoiceDetaill = (from DataRow dataRows in dataTableDetaillInvoice.Rows
                              select new InvoiceDetaill()
                              {
                                  id = Convert.ToInt32(dataRows["id"]),
                                  idinvoice = Convert.ToInt32(dataRows["client_bill_id"]),
                                  description = dataRows["description"].ToString(),
                                  price = Convert.ToInt32(dataRows["value"])
                              }).ToList();

            invoiceList = (from DataRow dataRow in dataTableInvoice.Rows
                           select new Invoice()
                           {
                               id = Convert.ToInt32(dataRow["id"]),
                               idclient = Convert.ToInt32(dataRow["client_id"]),
                               cod = Convert.ToInt32(dataRow["cod"]),
                               detaills = invoiceDetaill
                           }).ToList();

            return invoiceList;
        }

        // Insert users
        [HttpPost]
        public bool insertUser([FromBody] Client client)
        {
            var response = dataBase.operacionesSQL($"insert into client (name, last_name, bill_id) values ('{client.name}','{client.lastname}','{client.document}' );");
            return response;
        }

        // Insert invoice
        [HttpPost]
        public string insertInvoice([FromBody] Invoice invoice)
        {
            string sql = $"insert into invoice (client_id, cod) values ({invoice.idclient}, {invoice.cod});" + Environment.NewLine;
            foreach (InvoiceDetaill item in invoice.detaills)
            {
                sql += $"insert into invoicedetaill (client_bill_id, description, value) values ((select max(id) from invoice), '{item.description}', {item.price} );" + Environment.NewLine;
            }
            bool response = dataBase.operacionesSQL(sql);
            if (response)
            {
                return "Successfully";
            }
            else
            {
                return "Error";
            }
        }
    }
}