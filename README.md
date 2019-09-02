# Enegy-Ball
08/29 Unity道場復習 [Shader Graph]  Depth処理/ベクトル内積など


## エナジーボールの作り方と歪み処理に関して  

#### [1]半透明と不透明の接触部分に関して「深度バッファ」  
#### [2]球の外に近いほど明るくなる処理「ベクトルの内積」 
#### [3]UVスクロールとオーバーレイに関して  
#### [4]CameraOpaqueTextureによる歪み処理  


### [1]半透明と不透明の接触部分に関して「深度バッファ」 

Zバッファ(不透明オブジェクトまでのカメラからの距離)とカメラから実際のオブジェクト(対象への)の距離の差分を利用する。  
ようは必ず半透明のオブジェクトが必要。  

実際にやってみた。  

ん。。なんか違う、接触の判定は取れてるが。。。  

substractで反転させてみた。  

良い感じ。
こういうお試し的な処理がリアルタイムで観れるのはShader Graphのかなり強み。  

### [2]球の外に近いほど明るくなる処理「ベクトルの内積」 

Shader Graphにはズバリな処理をしてくれる「frenel」ノードがあるが、今回は「ベクトルの内積」での実装。  

「frenelノード」  

オブジェェクトからカメラへのベクトルの正規化した値から法線ベクトルの差し引きの値(向きの近似度)を利用する。  
完全に一致すれば1だし真逆の方向を向いていれば-1である。  

中の方を透明にしたいのでsubstractで反転するが。。。  
両面描画にしているので上手くいかない(片面と片面の数値が反転してしまってる為)  

よって、まず絶対値をとって値を揃えた上で反転してやる。  

ちょっとわかりにくいけどこんなスライドが使われてました。  

こんな感じ。  

### [3]UVスクロールとオーバーレイに関して  

UVスクロールに関しては基本Offsetを使う所は一緒だが人それぞれやり方が違いすぎて余り参考にならない。  
今回のスクロールはこんな感じ。  

これにさらにSinをいれたUVを足す。(UVで模様を作るのが手軽に出来るのもShader Graphならでは)  

肝となるのがブレンドの仕方。  
普通の乗算ではなく、Overlayを使う（コントラストをバリバリにして出るとこ出して後は背景っぽくできる。）  
Multiplyで効果を強くして、あとはMaximumで最低値を変える。  

こんな感じ。  

#### [4]CameraOpaqueTextureによる歪み処理   

2パス描けないので、歪みエフェクト用に新しいシェーダーを作る。  

今回学んだ一番の発見はこの「CameraOpaqueTexture」の処理。  
Texture2Dのパスをこれに帰るだけでRenderTexture(描画結果を貼り付けている)のような働きになる。  
めちゃくちゃ便利。  
後はこれのUVの値を弄るだけで色々な事が出来る。


