"use strict"

    // Добавление в корзину
    let addBasketBtn = document.querySelectorAll(".add-basket");

addBasketBtn.forEach((btn) => {
    btn.addEventListener("click", function () {
        let productId = parseInt(this.getAttribute("data-id"));

        fetch("https://localhost:7025/Home/AddProductToBasket?id=" + productId, {
            method: "POST",
            headers: {
                "Content-type": "application/json;charset=UTF-8"
            }
        })
            .then(response => response.text())
            .then(res => {
                document.querySelector(".basket-count-show").innerText = res;
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Товар добавлен в корзину",
                    showConfirmButton: false,
                    timer: 1500
                });
            });
    });
});




        //document.addEventListener('DOMContentLoaded', function () {
        //    const deleteButtons = document.querySelectorAll('.delete-basket-item');

        //    deleteButtons.forEach(button => {
        //        button.addEventListener('click', async function () {
        //            const productId = this.getAttribute('data-id');

        //            const response = await fetch('/Cart/DeleteAsync', {
        //                method: 'POST',
        //                headers: {
        //                    'Content-Type': 'application/json',
        //                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        //                },
        //                body: JSON.stringify({ id: parseInt(productId) })
        //            });

        //            if (response.ok) {
        //                const result = await response.json();

        //                // Обновить цену и количество на странице (если хотите)
        //                console.log("New total: " + result.total);
        //                console.log("Remaining count: " + result.count);

        //                // Перезагрузить страницу или удалить товар из DOM
        //                location.reload(); // Простой способ
        //            } else {
        //                alert("Ошибка при удалении товара.");
        //            }
        //        });
        //    });
        //});




let deleteBasketProducts = document.querySelectorAll(".delete-basket-item");

deleteBasketProducts.forEach((btn) => {
    btn.addEventListener("click", function () {
        let productId = parseInt(this.getAttribute("data-id"));
        let row = this.closest("tr"); // находим строку товара
        let countCell = row.querySelector("td:nth-child(4)"); // 4-я колонка — количество
        let currentCount = parseInt(countCell.innerText);

        fetch("https://localhost:7025/Cart/Delete?id=" + productId, {
            method: "POST"
        })
            .then(response => response.json())
            .then(res => {
                if (currentCount > 1) {
                    countCell.innerText = currentCount - 1; // уменьшаем на 1
                } else {
                    row.remove(); // полностью удаляем строку
                }

                // Обновляем общую сумму и количество
                document.querySelector(".total h1 span").innerText = res.total;
                document.querySelector(".basket-count-show").innerText = res.count;

                // Если корзина пуста — показываем сообщение и скрываем таблицу
                if (res.count === 0) {
                    document.querySelector(".cart-area").classList.add("d-none");
                    document.querySelector(".cart-empty-alert").classList.remove("d-none");
                }
            });
    });
});

  