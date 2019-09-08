using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using WpfAppMVVMConsumeWCFWebService.Models.Implementations;

namespace WpfAppMVVMConsumeWCFWebService
{
    // Serialisation et deserialisation -- https://www.c-sharpcorner.com/article/json-serialization-and-deserialization-in-c-sharp/

    public class MainViewModel
    {
        private DataContractJsonSerializer _dataContractJsonSerialize;

        #region -- Prperties --

        #endregion

        #region -- Commands --
        public DelegateCommand ValidateCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand GetConnectCommand { get; set; }
        public DelegateCommand UpdateCommand { get; set; } 
        #endregion

        #region -- Constructor --
        public MainViewModel()
        {
            LoginCommand = new DelegateCommand(Login, CanLogin);
            ValidateCommand = new DelegateCommand(Validate, CanValidate);
            UpdateCommand = new DelegateCommand(Update, CanUpdate);
            GetConnectCommand = new DelegateCommand(GetConnect, CanConnect);
        }
        #endregion

        #region -- Methodes --
        private bool CanUpdate(object obj)
        {
            return true;
        }

        private bool CanConnect(object obj)
        {
            return true;
        }

        private bool CanValidate(object parameter)
        {
            return true;
        }

        private bool CanLogin(object parameter)
        {
            return true;
        }

        private void Update(object obj)
        {

        }

        private void GetConnect(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearPasswordText = passwordBox.Password;

            string baseRequestUriString = @"http://localhost:64571/EmployeeService.svc/";
            string requestUriString = string.Format("{0}{1}{2}", baseRequestUriString, "GetConnect?value=", clearPasswordText);

            try
            {
                using (WebClient proxy = new WebClient())
                {
                    string serviceURL = string.Format(requestUriString, parameter);
                    byte[] data = proxy.DownloadData(serviceURL);

                    using (Stream stream = new MemoryStream(data))
                    {
                        DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(Employee));
                        var employee = obj.ReadObject(stream) as Employee;

                        var toUpdateEmploye = new Employee
                        {
                            EmployeeId = employee.EmployeeId,
                            EmployeEmail = employee.EmployeEmail,
                            EmployeName = employee.EmployeName,
                            EmployeSalary = employee.EmployeSalary
                        };
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void Login(object parameter)
        {
            // -- Using Json.NET --

            PasswordBox passwordBox = parameter as PasswordBox;
            string clearPasswordText = passwordBox.Password;
            try
            {
                //code for xml Response consumption from WCF rest Service[Start]
                string baseRequestUriString = @"http://localhost:64571/EmployeeService.svc/";
                string requestUriString = string.Format("{0}{1}{2}", baseRequestUriString, "GetById/", clearPasswordText);
                WebRequest webRequest = WebRequest.Create(requestUriString);
                webRequest.Method = "GET";
                webRequest.ContentType = @"application/json; charset=utf-8";

                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    string jsonResponse = string.Empty;

                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        jsonResponse = sr.ReadToEnd();

                        Console.WriteLine(jsonResponse);
                    }
                }

                #region -- Deserialization --  
                string json = "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}";

                using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    // Deserialization from JSON  
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Employee));
                    var bsObj2 = (Employee)deserializer.ReadObject(memoryStream);
                    //Response.Write("Name: " + bsObj2.Name); // Name: C-sharpcorner
                    //Response.Write("Description: " + bsObj2.Description); // Description: Share Knowledge  
                }
                #endregion
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        private void Validate(object obj)
        {
            string baseRequestUriString = @"http://localhost:64571/EmployeeService.svc/";
            string requestUriString = string.Format("{0}{1}", baseRequestUriString, "Add");

            var toUpdateEmploye = new Employee
            {
                EmployeeId = 104,
                EmployeEmail = "Employe102@compagny.com",
                EmployeName = "Employee 104",
                EmployeSalary = 3990
            };

            _dataContractJsonSerialize = new DataContractJsonSerializer(typeof(Employee));

            #region -- AddMethod --
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    _dataContractJsonSerialize.WriteObject(memoryStream, toUpdateEmploye);
                    string data = Encoding.UTF8.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Headers["Content-type"] = "application/json";
                        webClient.Encoding = Encoding.UTF8;
                        string adress = requestUriString;
                        webClient.UploadString(adress, "POST", data);
                    }

                    #region -- Serialisation --  https://www.c-sharpcorner.com/article/json-serialization-and-deserialization-in-c-sharp/
                    //BlogSite bsObj = new BlogSite()
                    //{
                    //    Name = "C-sharpcorner",
                    //    Description = "Share Knowledge"
                    //};

                    //DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(BlogSite));
                    //MemoryStream msObj = new MemoryStream();
                    //js.WriteObject(msObj, bsObj);
                    //msObj.Position = 0;
                    //StreamReader sr = new StreamReader(msObj);

                    //// "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}"  
                    //string json = sr.ReadToEnd();

                    //sr.Close();
                    //msObj.Close();
                    #endregion

                    Console.WriteLine("Employee Saved Successfully...");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
            #endregion           
        }

        #region --  *************************************************************  --
        private void PostCustomerResult()
        {
            HttpResponseMessage response = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri("http://pupanda-win10.fareast.corp.microsoft.com/RestService/Service1.svc/UpdateCustomer");
                    var toUpdateEmploye = new Employee
                    {
                        EmployeeId = 104,
                        EmployeEmail = "Employe102@compagny.com",
                        EmployeName = "Employee 104",
                        EmployeSalary = 3990
                    };

                    //var jsonRequest = JsonConvert.SerializeObject(toUpdateEmploye);
                    _dataContractJsonSerialize = new DataContractJsonSerializer(typeof(Employee));

                    //var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    ////response = await client.PostAsync(uri, stringContent);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private void btn_AddnewEmployee()
        {
            string requestUriString = @"http://localhost:64571/EmployeeService.svc/";
            requestUriString = string.Format("{0} {1}", requestUriString, "Add");

            var toUpdateEmploye = new Employee
            {
                EmployeeId = 104,
                EmployeEmail = "Employe102@compagny.com",
                EmployeName = "Employee 104",
                EmployeSalary = 3990
            };

            try
            {
                _dataContractJsonSerialize = new DataContractJsonSerializer(typeof(Employee));
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    _dataContractJsonSerialize.WriteObject(memoryStream, toUpdateEmploye);
                    string data = Encoding.UTF8.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);

                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Headers["Content-type"] = "application/json";
                        webClient.Encoding = Encoding.UTF8;
                        string adress = requestUriString;
                        webClient.UploadString(adress, "POST", data);

                        Console.WriteLine("Employee Saved Successfully...");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        /// <summary>
        /// -- OK http://localhost:62386/EmployeInfoService.svc/GetConnect?value=param --
        /// </summary>
        /// <param name="paramter"></param>
        private void ViewEmployeeDetail(string paramter)
        {
            string baseRequestUriString = @"http://localhost:64571/EmployeeService.svc/";
            string requestUriString = string.Format("{0}{1}{2}", baseRequestUriString, "GetConnect?value=", paramter);

            try
            {
                using (WebClient proxy = new WebClient())
                {
                    string serviceURL = string.Format(requestUriString, paramter);
                    byte[] data = proxy.DownloadData(serviceURL);

                    using (Stream stream = new MemoryStream(data))
                    {
                        DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(Employee));
                        var employee = obj.ReadObject(stream) as Employee;

                        var toUpdateEmploye = new Employee
                        {
                            EmployeeId = employee.EmployeeId,
                            EmployeEmail = employee.EmployeEmail,
                            EmployeName = employee.EmployeName,
                            EmployeSalary = employee.EmployeSalary
                        };
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        private void UpdateEmployee(string paramter)
        {
            string baseRequestUriString = @"http://localhost:64571/EmployeeService.svc/";
            string requestUriString = string.Format("{0}{1}", baseRequestUriString, "Add");

            var toUpdateEmploye = new Employee
            {
                EmployeeId = 104,
                EmployeEmail = "Employe102@compagny.com",
                EmployeName = "Employee 104",
                EmployeSalary = 3990
            };

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers["Content-type"] = "application/json";

                    using (MemoryStream memoriStream = new MemoryStream())
                    {
                        _dataContractJsonSerialize = new DataContractJsonSerializer(typeof(Employee));
                        _dataContractJsonSerialize.WriteObject(memoriStream, toUpdateEmploye);

                        // invoke the REST method  
                        client.UploadData(requestUriString, "PUT", memoriStream.ToArray());

                        Console.WriteLine("Employee updated Successfully...");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        private void DeleteEmployee(string paramter)
        {
            string baseRequestUriString = @"http://localhost:64571/EmployeeService.svc/";
            string requestUriString = string.Format("{0}{1}{2}", baseRequestUriString, "GetConnect?value=", paramter);

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers["Content-type"] = "application/json";

                    using (MemoryStream memoriStream = new MemoryStream())
                    {
                        _dataContractJsonSerialize = new DataContractJsonSerializer(typeof(string));
                        _dataContractJsonSerialize.WriteObject(memoriStream, paramter);

                        // invoke the REST method  
                        byte[] data = client.UploadData(requestUriString, "DELETE", memoriStream.ToArray());

                        Console.WriteLine("Employee deleted Successfully...");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }
        #endregion 
        #endregion
    }
}
