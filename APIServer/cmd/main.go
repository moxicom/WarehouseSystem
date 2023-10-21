package main

import (
	"APIServer/internal/db"
	"APIServer/internal/middleware"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func main() {
	db, err := db.OpenDB()
	if err != nil {
		log.Fatal(err)
	}

	router := gin.Default()

	router.GET("/auth", middleware.LoginMiddleware(db), func(ctx *gin.Context) {
		user, _ := ctx.Get("user")
		ctx.JSON(http.StatusOK, user)
	})

	router.Run(":8080")
}
