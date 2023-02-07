window.onload = function () {
    const basketId = document.querySelector('main').dataset.basketId;
    const basketAmountLbl = document.querySelector('#header-basket-amount');
    const basketAddBtns = document.querySelectorAll('.js-add-btn');
    const basketRemoveBtns = document.querySelectorAll('.js-remove-btn');
    const originalTotalSection = document.querySelector('#original-total-section');
    const originalTotalLbl = document.querySelector('#original-total');
    const currentTotalSection = document.querySelector('#current-total-section');
    const currentTotalLbl = document.querySelector('#current-total');
    const voucherInputSection = document.querySelector('#voucher-input-section');
    const voucher = document.querySelector('#voucher');
    const voucherApplyBtn = document.querySelector('#apply-voucher');
    const voucherRemoveBtn = document.querySelector('#remove-voucher');
    const voucherAlert = document.querySelector('#voucher-applied-section');
    const voucherName = document.querySelector('#voucher-name');

    const calcNewTotal = (orig, discount) => {
        const newTotal = orig - (orig * (discount / 100));
        return newTotal.toFixed(2);
    };

    let setBasketIdCookie = function (val) {
        var date = new Date();
        date.setTime(date.getTime() + (99 * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
        document.cookie = "BasketId=" + (val) + expires + "; path=/";
        console.log(`basketId = ${val}`);
    }

    setBasketIdCookie(basketId);

    if (basketAddBtns.length) {
        basketAddBtns.forEach(btn => {
            btn.addEventListener('click', e => {
                e.preventDefault();
                let addUrl = e.target.dataset.addUrl;
                console.log(`clicked add btn with url ${addUrl}`);
                fetch(e.target.dataset.addUrl, {
                    method: 'POST',
                })
                    .then(response => {
                        if (response.ok) {
                            var basketAmount = parseInt(basketAmountLbl.innerHTML);
                            basketAmount++;
                            basketAmountLbl.innerHTML = basketAmount;
                        }
                    });
            })
        });
    }

    if (basketRemoveBtns.length) {
        basketRemoveBtns.forEach(btn => {
            btn.addEventListener('click', e => {
                e.preventDefault();
                let removeUrl = e.target.dataset.removeUrl;
                console.log(`clicked remove btn with url ${removeUrl}`);
                fetch(e.target.dataset.removeUrl, {
                    method: 'POST',
                })
                    .then(response => {
                        if (response.ok) {
                            var basketAmount = parseInt(basketAmountLbl.innerHTML);
                            basketAmount--;
                            basketAmountLbl.innerHTML = basketAmount;

                            var originalTotal = parseFloat(originalTotalLbl.innerHTML);
                            var productPrice = parseFloat(e.target.dataset.price);
                            originalTotal -= productPrice;
                            originalTotalLbl.innerHTML = originalTotal.toFixed(2);

                            e.target.closest('tr').remove();
                        }
                    });
            })
        });
    }

    if (voucherApplyBtn) {
        voucherApplyBtn.addEventListener('click', e => {
            var applyUrl = e.target.dataset.addVoucherUrl;
            var voucherCode = voucher.value;
            if (voucherCode.length > 0) {
                console.log('applying voucher code ' + voucherCode + ' to basket ' + basketId);
                applyUrl = applyUrl.replace('{0}', basketId).replace('{1}', voucherCode);
                fetch(applyUrl, {
                    method: 'POST',
                })
                    .then(response => response.json())
                    .then(voucherData => {
                        voucherName.innerHTML = voucherData.name;
                        var originalTotal = parseFloat(originalTotalLbl.innerHTML);
                        currentTotalLbl.innerHTML = calcNewTotal(originalTotal, parseInt(voucherData.discountPercentage));
                        voucherAlert.removeAttribute('hidden');
                        currentTotalSection.removeAttribute('hidden');
                        voucherInputSection.setAttribute('hidden', 'hidden');
                    });
            }
        });
    }

    if (voucherRemoveBtn) {
        voucherRemoveBtn.addEventListener('click', e => {
            var removeUrl = e.target.dataset.removeVoucherUrl;
            console.log('removing voucher code from basket ' + basketId);
            removeUrl = removeUrl.replace('{0}', basketId);
            console.log(`remove url = ${removeUrl}`);
            fetch(removeUrl, {
                method: 'POST',
            })
                .then(response => {
                    console.log(response);
                    voucherName.innerHTML = '';
                    currentTotalLbl.innerHTML = '';
                    voucherAlert.setAttribute('hidden', 'hidden');
                    currentTotalSection.setAttribute('hidden', 'hidden');
                    voucherInputSection.removeAttribute('hidden');
                });
        });
    }
};