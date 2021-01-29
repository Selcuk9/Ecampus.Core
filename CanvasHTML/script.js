
let sz = 30;//размер вирутальной клетки

let w = 27;//ширина пропуска в клетках
let h = 17;//высота пропуска в клетках


canvas.width = w * sz;
canvas.height = h * sz;

draw();

function draw(){
     //основная канва
     var canvas = document.getElementById('canvas');
     var ctx = canvas.getContext('2d');

     //весь фон
     ctx.fillStyle = '#163E73';
     ctx.fillRect(0, 0, w * sz, h * sz);

     //формируем картинку на фоне
     var bkgImg = new Image;
     bkgImg.src = "/files/logo.svg";
     //что длаем после загруки фоновой картинки
     bkgImg.onload = function () {
        ctx.drawImage(bkgImg, sz * 16, sz * -1);
        
        //белая полоса
        ctx.fillStyle = "white";
        ctx.fillRect(0,1*sz,w*sz, 2 * sz);

        let f = 25;
        ctx.font = f + "px Arial";
        ctx.fillStyle = "#163E73";
        ctx.textAlign = "center";
        ctx.fillText("СЕВЕРО-КАВКАЗСКИЙ ФЕДЕРАЛЬНЫЙ УНИВЕРСИТЕТ", sz * w / 2, sz * 2 + f / 2);

        
        ctx.fillStyle = "white";
        ctx.fillRect(1 * sz, 4 * sz, 9 * sz, 11 * sz);

        let b = 10;
        ctx.fillStyle = "green";
        ctx.fillRect(1 * sz + b, 4 * sz + b, 9 * sz - b * 2, 11 * sz - b * 2);

        var img = new Image;
        img.src = "/files/userpic.png";
        img.onload = function () {
            ctx.drawImage(img, 1 * sz + b, 4 * sz + b, 9 * sz - b * 2, 11 * sz - b * 2);
        }

     }


}