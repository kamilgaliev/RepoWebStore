﻿Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: "",
        decrementLink: "",
        removeFromCartLink: ""
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);
        Cart.initEvents();
    },

    initEvents: function () {
        $(".add-to-cart").click(Cart.addToCart);

        $(".cart_quantity_up").click();
        $(".cart_quantity_down").click(Cart.incrementItem);
        $(".cart_quantity_delete").click();
    },

    addToCart: function (event) {
        event.preventDefault();

        var button = $(this);
        const id = button.data("id");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                Cart.showToolTip(button);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("addToCart fail"); });
    },

    showToolTip: function (button) {
        button.tooltip({ title: "Добавлено в корзину!" }).tooltip("show");
        setTimeout(function () {
            button.tooltip("destroy");
        },500);
    },

    refreshCartView: function () {
        var container = $("#cart-container");
        $.get(Cart._properties.getCartViewLink)
            .done(function (cartHtml) {
                container.html(cartHtml);
            })
            .fail(function () { console.log("refreshCartView fail"); });
    },

    incrementItem: function (event) {
        event.preventDefault();

        var button = $(this);
        const id = button.data("id");

        var tr = button.closest("tr");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                const count = parseInt($(".cart_quantity_input", tr).val());
                $(".cart_quantity_input", tr).val(count + 1);

                Cart.refreshPrice(tr);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("incrementItem fail"); });
    },

    refreshPrice: function (container) {
        const count = parseInt($(".cart_quantity_input", container).val());
        const price = parseFloat($(".cart_price", container).data("price"));

        const totalPrice = count * price;

        const value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
        const cartTotalPrice = $(".cart_total_price", container);

        cartTotalPrice.data("price", totalPrice);
        cartTotalPrice.html(value);

        Cart.refreshTotalPrice();
    },

    refreshTotalPrice: function () {

        var totalPrice = 0;

        $(".cart_total_price").each(function () {
            const price = parseFloat($(this).data("price"));
            totalPrice += price;
        });

        const value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
        $("#total-oder-price").html(value);
    }
}