# narozip2epub

「小説家になろう (https://syosetu.com/)」（以後「なろう」と表記します）から、TXTダウンロードを指定してダウンロードしたzipファイルを縦書きのEPUBファイルに変換します。
このEPUBファイルは、Amazonの「Send to Kindle (https://www.amazon.co.jp/sendtokindle)」を介してKindle端末へ転送し閲覧することができます。

# narozip2epub開発の動機

私は「なろう」で公開されているある小説をKindle端末で縦書きとして読みたいと思いました。
「なろう」から一括ダウンロードしたzipファイルは各エピソード毎に1つのテキストファイルとしたものが格納されています。

これをWORDに読み込んでレイアウトを縦書きとしたdocxとして保存し、「Send to Kindle」を介してKindle端末に転送することで目的を果たせそうでした。
ところが、Kindle端末上でのページの送り・戻しのタッチ操作が横書きの左綴じ本のままになっていました。

慣れればKindleの画面中央をタッチすることでページ送りに対する違和感は無くなるのですが、とっさにページを戻ろうとしたときにKindleの画面右端をタッチするとページが戻らずページが進んでしまい現在位置を見失ってしまいます。

これに耐え切れず調べたところアラビア系の言語をインストールすることで、docxで本の読み進める方向（横書きでも右から読む言語もあるので、縦書き＝右→左へと読むとは限らない）の指定ができることがわかりました。
しかし、この指定でWORD上で見開きページの並び順が期待値となったものの「Send to Kindle」を介してKindleへ送った本のページ送り操作は「左→右」へ読み進める状態のままでした。

Amazon Kindle本とて販売されている縦書きの本は、Kindle端末上で「右→左」へ読み進める操作が出来ているので、「なろう」のテキストファイルを縦書きのEPUB形式へ変換すればなんとかなると考えました。

ところが、テキストファイルを読み込んでEPUBとして編集できるソフトウェアはあるものの、自動で変換できるものは見つかりませんでした。

自力でテキストファイルをEPUB形式へ変換するソフトウェアを開発するべくEPUB形式について調べたところ、大雑把に言えば「EPUBとはhtmlやcssをzipで固めたもの」ということが分かり、それならばとnarozip2epubが出来ました。

# 参考にしたEPUBの資料

以下のサイトで公開されている資料を参考にしました。

一般社団法人 デジタル出版社連盟  
電書協 EPUB 3 制作ガイド  
https://dpfj.or.jp/counsel/guide  
「電書協 EPUB 3 制作ガイド ver.1.1.3 2015年1月1日版」

私の目的は、EPUBの仕様を活用した素晴らしい本を出版することではなく、テキストファイル群を自動で縦書きの本のすること、なので最低限の仕様だけみてオリジナルのEPUBの仕様やその翻訳（https://imagedrive.github.io/spec/）は参考としていません。

# 使い方

narozip2epub はWindowsのコマンドラインで使用します。

## コマンドラインオプション

nazozip2epub <optoins>

options:
  --output-dir EPUBファイル群の出力先ディレクトリ  
  --title 本のタイトル  
  --kana-title カタカナのタイトル。「制作ガイド」では全角カタカナを使用しています。端末上でソートで使われるらしいので記号などは入れない方が無難かと思います。  
  --author 著者  
  --kana-author カタカナの著者名。「制作ガイド」では全角カタカナを使用しています。端末上でソ  ートで使われるらしいので記号などは入れない方が無難かと思います。  
  --source-zip 「なろう」からTXTダウンロードしたzipファイル。  
  
zipをEPUBへ変換するにあたって、narozip2epubは適当なサイズで分割しEPUBを出力ます。以後、分割した単位をボリュームと表記します。
1つのzipから、複数のボリュームに分割したEPUBを出力するため、出力先はファイル名ではなくディレクトリで指定します。
出力されるファイルは、「本のタイトル」に続いて「(ボリューム番号)」が付いたものになります。

例：  
narozip2epub --outputdir サンプル本 --title サンプル本 --kana-title サンプルボン --author 名無し --kana-author ナナシ --source.zip sample.zip

# nazozip2eubが参照しているライブラリ

CommandLineParser 2.9.1 https://github.com/commandlineparser/commandline   
System.CodeDom 9.0.1 https://dotnet.microsoft.com/ja-jp/  
System.Drawing.Common 9.0.1 https://github.com/dotnet/winforms  

# narozip2epubのライセンス

MIT
