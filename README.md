# Enegy-Ball
08/29 Unity道場復習 [Shader Graph]  Depth処理/ベクトル内積など

![EnegyBall](https://user-images.githubusercontent.com/43961147/64123557-acae0e80-cddf-11e9-85a8-b5fd617f23f9.gif)

## エナジーボールの作り方と歪み処理に関して  

#### [1]半透明と不透明の接触部分に関して「深度バッファ」  
#### [2]球の外に近いほど明るくなる処理「ベクトルの内積」 
#### [3]UVスクロールとオーバーレイに関して  
#### [4]CameraOpaqueTextureによる歪み処理  

#### 全体図  

<img width="700" alt="ball1" src="https://user-images.githubusercontent.com/43961147/64123640-e8e16f00-cddf-11e9-83d2-034ef773f89b.png">

### [1]半透明と不透明の接触部分に関して「深度バッファ」 

<img width="700" alt="ball2" src="https://user-images.githubusercontent.com/43961147/64123643-ec74f600-cddf-11e9-9d81-c527e87e4477.png">

Zバッファ(不透明オブジェクトまでのカメラからの距離)とカメラから実際のオブジェクト(対象への)の距離の差分を利用する。  
ようは必ず半透明のオブジェクトが必要。  

実際にやってみた。  


<img width="570" alt="ball3" src="https://user-images.githubusercontent.com/43961147/64123649-f139aa00-cddf-11e9-91a0-f8a15315d1ad.png">

ん。。なんか違う、接触の判定は取れてるが。。。  

substractで反転させてみた。  

<img width="700" alt="ball4" src="https://user-images.githubusercontent.com/43961147/64123655-f4349a80-cddf-11e9-96af-5cd6e8ba4fae.png">

<img width="570" alt="ball5" src="https://user-images.githubusercontent.com/43961147/64123678-06163d80-cde0-11e9-8df5-f12232758266.png">

良い感じ。
こういうお試し的な処理がリアルタイムで観れるのはShader Graphのかなり強み。  

### [2]球の外に近いほど明るくなる処理「ベクトルの内積」 

<img width="700" alt="ball7" src="https://user-images.githubusercontent.com/43961147/64123686-0f9fa580-cde0-11e9-9e75-93dec1600ec2.png">

Shader Graphにはズバリな処理をしてくれる「frenel」ノードがあるが、今回は「ベクトルの内積」での実装。  

「frenelノード」  
<img width="300" alt="ball6" src="https://user-images.githubusercontent.com/43961147/64123658-f72f8b00-cddf-11e9-8795-ae8944c0a283.png">


オブジェェクトからカメラへのベクトルの正規化した値から法線ベクトルの差し引きの値(向きの近似度)を利用する。  
完全に一致すれば1だし真逆の方向を向いていれば-1である。  

中の方を透明にしたいのでsubstractで反転するが。。。  
両面描画にしているので上手くいかない(片面と片面の数値が反転してしまってる為)  

よって、まず絶対値をとって値を揃えた上で反転してやる。  

ちょっとわかりにくいけどこんなスライドが使われてました。  
![ball8](https://user-images.githubusercontent.com/43961147/64123694-14fcf000-cde0-11e9-97b3-84b1006e186d.png)

こんな感じ。  

<img width="570" alt="ball9" src="https://user-images.githubusercontent.com/43961147/64123702-19290d80-cde0-11e9-9ef4-2cc3fed96c91.png">

### [3]UVスクロールとオーバーレイに関して  

UVスクロールに関しては基本Offsetを使う所は一緒だが人それぞれやり方が違いすぎて余り参考にならない。  
今回のスクロールはこんな感じ。  

<img width="700" alt="ball10" src="https://user-images.githubusercontent.com/43961147/64123709-1c23fe00-cde0-11e9-8bf8-1bdd59558bb8.png">

これにさらにSinをいれたUVを足す。(UVで模様を作るのが手軽に出来るのもShader Graphならでは)  

<img width="700" alt="ball11" src="https://user-images.githubusercontent.com/43961147/64123715-1f1eee80-cde0-11e9-9446-1185612e990b.png">

肝となるのがブレンドの仕方。  
普通の乗算ではなく、Overlayを使う（コントラストをバリバリにして出るとこ出して後は背景っぽくできる。）  
Multiplyで効果を強くして、あとはMaximumで最低値を変える。  

<img width="260" alt="ball12" src="https://user-images.githubusercontent.com/43961147/64123724-234b0c00-cde0-11e9-8003-2afafcbc93f3.png">

こんな感じ。  

<img width="570" alt="ball13" src="https://user-images.githubusercontent.com/43961147/64123732-2514cf80-cde0-11e9-817d-00d1d3d3ad90.png">

#### [4]CameraOpaqueTextureによる歪み処理   

<img width="750" alt="ball15" src="https://user-images.githubusercontent.com/43961147/64124432-fc8dd500-cde1-11e9-872f-b4d75a40d772.png">

2パス描けないので、歪みエフェクト用に新しいシェーダーを作る。  

今回学んだ一番の発見はこの「CameraOpaqueTexture」の処理。  
Texture2Dのパスをこれに帰るだけでRenderTexture(描画結果を貼り付けている)のような働きになる。（対象に貼るにはどうやらScreenPositionが必要）  
めちゃくちゃ便利。  
後はこれのUVの値を弄るだけで色々な事が出来る。



