window.onload = function () {
    var pTags = document.querySelectorAll(".chonViTri");
    var getLink = document.querySelector(".seatPlanButton");
    for (var i = 0; i < pTags.length; i++) {
        pTags[i].addEventListener('click', function () {
            var link = this.getAttribute('data-id');

            if (!isSignedIn) {  // Use the JavaScript variable
                getLink.setAttribute('href', '/chon-ghe/lich-chieu/'+link);
            } else {
                getLink.setAttribute('href', '/chon-ghe/lich-chieu/' + link);
                console.log('data-vi-tri' + link);
            }
        });
    }
} 