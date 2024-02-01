package handlers

import (
	"APIServer/internal/repository"

	"github.com/gin-gonic/gin"
)

type handler struct {
	r repository.Repository
}

func NewHandlers(r repository.Repository) *handler {
	return &handler{r}
}

func (h *handler) InitRoutes() *gin.Engine {
	router := gin.Default()

	// auth
	router.GET("/auth", h.r.LoginMiddleware(), h.authHandler)

	users := router.Group("/users")
	{
		users.GET("/", h.r.CommonMiddleware(), func(ctx *gin.Context) {
			h.getAllUsers(ctx)
		})

		users.POST("/", func(ctx *gin.Context) {
			// h.r.DataMW[models.User](),
			h.insertUser(ctx)
		})

		users.DELETE("/:user_id", h.r.CommonMiddleware(), func(ctx *gin.Context) {
			h.deleteUserByID(ctx)
		})

		users.PUT("/:user_id", func(ctx *gin.Context) {
			// h.r.DataMW[models.User]()
			h.updateUser(ctx)
		})
	}

	// categories
	categories := router.Group("/categories")
	{
		categories.GET("/", h.r.CommonMiddleware(), func(ctx *gin.Context) {
			h.getAllCategoriesHandler(ctx)
		})

		categories.GET("/:category_id", h.r.CommonMiddleware(), func(ctx *gin.Context) {
			h.getCategoryHandler(ctx)
		})

		categories.GET("/:category_id/title", h.r.CommonMiddleware(), func(ctx *gin.Context) {
			h.getCategoryTitle(ctx)
		})

		categories.POST("/", func(ctx *gin.Context) {
			// h.r.DataMW[models.Category]()
			h.insertCategory(ctx)
		})

		categories.DELETE("/:category_id", h.r.CommonMiddleware(), func(ctx *gin.Context) {
			h.deleteCategory(ctx)
		})

		categories.PUT("/:category_id", func(ctx *gin.Context) {
			// h.r.DataMW[models.Category](),
			h.updateCategory(ctx)
		})
	}

	// items
	items := router.Group("/items")
	{
		items.DELETE("/:item_id", h.r.CommonMiddleware(), func(ctx *gin.Context) {
			h.deleteItem(ctx)
		})

		items.POST("/", func(ctx *gin.Context) {
			// h.r.DataMW[models.Item](),
			h.insertItem(ctx)
		})

		items.PUT("/:item_id", func(ctx *gin.Context) {
			// h.r.DataMW[models.Item](),
			h.updateItem(ctx)
		})
	}
	return router
}
