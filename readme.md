# WIREHOUSE system
Course project by @moxicom

# General info
* Client - C# WPF MVVM using RestSharp
* Server - Golang gin
* Database - postgres

![image](https://github.com/moxicom/WarehouseSystem/blob/main/PresentationFiles/Pres_1.gif)
  
# How to run
### Client
* Replace `baseUrl` with the one you need in `App.xaml.cs` file
* Build and run application

### Server
* Set correct db conection in `APIServer/internal/db/db_functions.go` file
* Run `cmd/main.go`

# REST API requests:
* GET `/auth`
* GET `/users`
* DELETE `/users/:user_id`
* POST `/users`
* PUT `/users/:user_id`
* GET `/categories`
* GET `/categories/:category_id`
* GET `/categories/:category_id/title`
* POST `/categories`
* DELETE `/categories/:category_id`
* PUT `/categories/:category_id`
* DELETE `/items/:item_id`
* POST `/items`
* PUT `/items/item_id`

