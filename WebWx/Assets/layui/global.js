
/*!
 * layui ����
 */

layui.define(['code', 'element', 'table', 'util', 'carousel', 'laytpl'], function(exports){
  var $ = layui.jquery
  ,element = layui.element
  ,layer = layui.layer
  ,form = layui.form
  ,util = layui.util
  ,carousel = layui.carousel
  ,laytpl = layui.laytpl
  ,device = layui.device()

  ,$win = $(window), $body = $('body');

  //ban iframe
  //;!function(){self!==parent&&(location.href="//www.baidu.com/")}();


  //��ֹ IE7 ���·���
  if(device.ie && device.ie < 8){
    layer.alert('Layui ���֧�� IE8��������ǰʹ�õ��ǹ��ϵ� IE'+ device.ie + '�����齫�᲻�ѣ�');
  }

  var home = {
    //��ȡ�߼������
    getBrowser: function(){
      var ua = navigator.userAgent.toLocaleLowerCase()
      ,mimeType = function(option, value) {
        var mimeTypes = navigator.mimeTypes;
        for (var key in mimeTypes) {
          if (mimeTypes[key][option] && mimeTypes[key][option].indexOf(value) !== -1) {
            return true;
          }
        }
        return;
      };
      if(ua.match(/chrome/)){
        if(mimeType('type', '360') || mimeType('type', 'sogou')) return;
        if(ua.match(/edg\//)) return 'edge';
        return 'chrome'
      } else if(ua.match(/firefox/)){
        return 'firefox';
      }
      
      return;
    }
  };

  var elemHome = $('#LAY_home');
  var local = layui.data('layui');

  //��ʼ����
  layer.ready(function(){

    //������ʾ
    if(local.version && local.version !== layui.v){
      layer.open({
        type: 1
        ,title: '������ʾ' //����ʾ������
        ,closeBtn: false
        ,area: '300px;'
        ,shade: false
        ,offset: 'b'
        ,id: 'LAY_updateNotice' //�趨һ��id����ֹ�ظ�����
        ,btn: ['������־', '��֪����']
        ,btnAlign: 'c'
        ,moveType: 1 //��קģʽ��0����1
        ,content: ['<div class="layui-text">'
          ,'layui �ѷ����°汾����<strong style="padding-right: 10px; color: #fff;">v'+ layui.v + '</strong>'
        ,'</div>'].join('')
        ,skin: 'layui-layer-notice'
        ,yes: function(index){
          layer.close(index);
          setTimeout(function(){
            location.href = '/doc/base/changelog.html';
          }, 500);
        }
        ,end: function(){
          layui.data('layui', {
            key: 'version'
            ,value: layui.v
          });
        }
      });
    }
    layui.data('layui', {
      key: 'version'
      ,value: layui.v
    });
  });

  //ͷ������
  ;!function(){
    var elemComponentSelect = $(['<select lay-search lay-filter="component">'
      ,'<option value="">�����������</option>'
      ,'<option value="element/layout.html">grid դ�񲼾�</option>'
      ,'<option value="element/layout.html#admin">layout admin ����</option>'
      ,'<option value="element/color.html">color ��ɫ</option>'
      ,'<option value="element/icon.html">iconfont ����ͼ��</option>'
      ,'<option value="base/element.html#css">font �����С ��ɫ</option>'
      ,'<option value="element/anim.html">animation ����</option>'
      ,'<option value="element/button.html">button ��ť</option>'
      ,'<option value="element/form.html">form ����</option>'
      ,'<option value="element/form.html#input">input �����</option>'
      ,'<option value="element/form.html#select">select ����ѡ���</option>'
      ,'<option value="element/form.html#checkbox">checkbox ��ѡ��</option>'
      ,'<option value="element/form.html#switch">switch ����</option>'
      ,'<option value="element/form.html#radio">radio ��ѡ��</option>'
      ,'<option value="element/form.html#textarea">textarea �ı���</option>'
      ,'<option value="element/nav.html">nav �����˵�</option>'
      ,'<option value="element/menu.html">menu ����ͨ�ò˵�</option>'
      ,'<option value="element/nav.html#breadcrumb">breadcrumb ���м</option>'
      ,'<option value="element/tab.html">tabs ѡ�</option>'
      ,'<option value="element/progress.html">progress ������</option>'
      ,'<option value="element/panel.html">panel ��� / card</option>'
      ,'<option value="element/collapse.html">collapse �۵����/�ַ���</option>'
      ,'<option value="element/table.html">table ���Ԫ��</option>'
      ,'<option value="element/badge.html">badge ����</option>'
      ,'<option value="element/timeline.html">timeline ʱ����</option>'
      ,'<option value="element/auxiliar.html#blockquote">blockquote ���ÿ�</option>'
      ,'<option value="element/auxiliar.html#fieldset">fieldset �ֶμ�</option>'
      ,'<option value="element/auxiliar.html#hr">hr �ָ���</option>'
      
      ,'<option value="modules/layer.html">layer ������/�����ۺ�</option>'
      ,'<option value="modules/laydate.html">laydate ����ʱ��ѡ����</option>'
      ,'<option value="modules/laypage.html">laypage ��ҳ</option>'
      ,'<option value="modules/laytpl.html">laytpl ģ������</option>'
      ,'<option value="modules/table.html">table ���ݱ��</option>'
      ,'<option value="modules/form.html">form ��ģ��</option>'
      ,'<option value="modules/upload.html">upload �ļ�/ͼƬ�ϴ�</option>'
      ,'<option value="modules/dropdown.html">dropdown �����˵�</option>'
      ,'<option value="modules/dropdown.html#contextmenu">contextmenu �Ҽ��˵�</option>'
      ,'<option value="modules/transfer.html">transfer �����</option>'
      ,'<option value="modules/tree.html">tree ���β˵�</option>'
      ,'<option value="modules/colorpicker.html">colorpicker ��ɫѡ����</option>'
      ,'<option value="modules/element.html">element ����Ԫ�ز���</option>'
      ,'<option value="modules/slider.html">slider ����</option>'
      ,'<option value="modules/rate.html">rate ����</option>'
      ,'<option value="modules/carousel.html">carousel �ֲ�/�����</option>'
      ,'<option value="modules/layedit.html">layedit ���ı��༭��</option>'
      ,'<option value="modules/flow.html">flow ��Ϣ��/ͼƬ������</option>'
      ,'<option value="modules/util.html">util ���߼�</option>'
      ,'<option value="modules/code.html">code �����ı�������</option>'

      ,'<option value="/layim/">layim</option>'
      ,'<option value="/layuiadmin/">layuiadmin</option>'
    ,'</select>'
    ,'<i class="layui-icon layui-icon-search"></i>'].join(''));

    $('.component').append(elemComponentSelect);
    form.render('select', 'LAY-site-header-component');

    //�������
    form.on('select(component)', function(data){
      debugger
      var value = data.value;
      location.href = /^\//.test(value) ? value : ('/web/doc/'+ value);
    });
  }();

  //�����ֲ� TIPS
  var notice = function(options, elemParter){
    var local = layui.data('layui');

    options = options || {};

    if(device.mobile) return;

    //�Ƿ���ʾ tips
    var keyName = 'notice_topnav_'+ options.key
    ,notParter = local[keyName] && new Date().getTime() - local[keyName] < (options.tipsInterval || 1000*60*30); //Ĭ�� 30 ���ӳ���һ��

    if(!options.tips) layer.close(layer.tipsIndex);

    if(!notParter && options.tips){
      var tipsIndex = layer.tipsIndex = layer.tips(
        ['<a href="'+ options.url +'" target="_blank" style="display: block; line-height: 30px; padding: 10px; text-align: center; font-size: 14px; background-image: linear-gradient(to right,#8510FF,#D025C2,#FF8B2D,#FF0036); color: #fff; '+ (options.tipsCss || '') +'">' 

          //�����ƾ��䣺background-image: linear-gradient(to right,#8510FF,#D025C2,#FF8B2D,#FF0036);
          //�����ƻ��background-image: linear-gradient(to right,#8510FF,#D025C2,#F64E2C,#FF0036);
          
          //��Ѷ���䣺background-image: linear-gradient(to right,#1242A4,#1746A1,#CFAE71,#1746A1);

          ,options.desc || ''
        ,'</a>'].join('')
        ,elemParter
        ,{
          tips: (options.tipsStyle ? new Function('return '+ options.tipsStyle)() : [3, '#9F17E9'])

          //�����ƾ��䣺[3, '#9F17E9']
          //��Ѷ�ƾ��䣺[3, '#1443A3'] //[3, '#803ED9'] 

          ,skin: 'layui-hide-xs'
          ,maxWidth: 320
          ,time: 0
          ,anim: 5
          ,tipeMore: true
          ,success: function(layero, index){
            layero.find('.layui-layer-content').css({
              'padding': 0
            });
            layero.find('a').on('click', function(){
              elemParter.trigger('click');
            });

            //����С��ͷ
            var tipsG = layero.find('.layui-layer-TipsG');
            if(tipsG.css('left') !== '5px'){
              tipsG.hide();
            }

            //�ƶ�����ʽ
            if(elemParter.parent().css('display') === 'none'){
              layero.css({
                left: '50%'
                ,top: '80px'
                ,'margin-left': -(layero.width()/2)
              });
              tipsG.hide();
            }
          }
        }
      )
      //�������
      elemParter.on('click', function(){
        layui.data('layui', {
          key: keyName
          ,value: new Date().getTime()
        });
        layer.close(tipsIndex);
      });
    }

  };

  //����¼�
  var events = {
    //��ϵ��ʽ
    contactInfo: function(){
      layer.alert('<div class="layui-text">���к������򣬿���ϵ��<br>���䣺</div>', {
        title:'��ϵ'
        ,btn: false
        ,shadeClose: true
      });
    }
    //���ں�
    ,weixinmp: function(){
      layer.photos({
        photos: {
          data: [{
            alt: 'layui ���ں�'
            ,"src": "" //ԭͼ��ַ
            ,"thumb": "" //����ͼ��ַ
          }]
        }
      })
    }
  };

  $body.on('click', '*[site-event]', function(){
    var othis = $(this)
    ,attrEvent = othis.attr('site-event');
    events[attrEvent] && events[attrEvent].call(this, othis);
  });


  //�л��汾
  form.on('select(tabVersion)', function(data){
    var value = data.value;
    location.href = value === 'new' ? '/' : ('/' + value + '/doc/');
  });
  

  //��ҳ banner
  setTimeout(function(){
    $('.site-zfj').addClass('site-zfj-anim');
    setTimeout(function(){
      $('.site-desc').addClass('site-desc-anim')
    }, 5000)
  }, 100);


  //����ǰ�ò���
  var digit = function(num, length, end){
    var str = '';
    num = String(num);
    length = length || 2;
    for(var i = num.length; i < length; i++){
      str += '0';
    }
    return num < Math.pow(10, length) ? str + (num|0) : num;
  };


  //���ص���ʱ
  var setCountdown = $('#setCountdown');
  if($('#setCountdown')[0]){
    $.get('/api/getTime', function(res){
      util.countdown(new Date(2017,7,21,8,30,0), new Date(res.time), function(date, serverTime, timer){
        var str = digit(date[1]) + ':' + digit(date[2]) + ':' + digit(date[3]);
        setCountdown.children('span').html(str);
      });
    },'jsonp');
  }


  //Adsense
  ;!function(){
    var len = $('.adsbygoogle').length;
    try {
      for(var i = 0; i < len; i++){
        (adsbygoogle = window.adsbygoogle || []).push({});
      }
    } catch (e){
      console.error(e)
    }
  }();
  


  //չʾ��ǰ�汾
  $('.site-showv').html(layui.v);

  //��ȡGithub����
  var getStars = $('#getStars');
  if(getStars[0]){
    var res = {"stargazers_count":'500'}
      getStars.html(res.stargazers_count);
  }
  
  //��ҳ����
  (function(){
    var elemDowns = $('.site-showdowns');
    //��ȡ������
    if(elemDowns[0]){
          var res = {"number":2356878,"title":"layui������"};
          elemDowns.html(res.number);
    }
    
    //��¼����
    $('.site-down').on('click',function(e){
      var othis = $(this)
      ,local = layui.data('layui')
      ,setHandle = function(){

      };
      if(!local.disclaimer){
        e.preventDefault();

        layer.confirm([
          '<div class="layui-text" style="padding: 10px 0;">'
          ,'�����Ķ���<a href="/www.layui.com/about/disclaimer.html" target="_blank">layui ��Դ��������������</a>��'
          ,'���ٽ�������</div>'
        ].join(''), {
          title: '������ʾ'
          ,btn: ['���Ķ�', 'ȡ��']
          ,maxWidth: 750
        }, function(index){
          layui.data('layui', {
            key: 'disclaimer'
            ,value: new Date().getTime()
          });
          layer.close(index);

          othis[0].click();
          setHandle();
        });
      } else {
        setHandle();
      }
    });
  })();
  
  
  //�̶�Bar
  util.fixbar({
    showHeight: 60
    ,css: function(){
      if(global.pageType === 'demo'){
        return {bottom: 75}
      }
    }()
  });
  
  //����scroll
  ;!function(){
    var main = $('.site-menu'), scroll = function(){
      var stop = $(window).scrollTop();

      if($(window).width() <= 992) return;
      var bottom = 0;

      if(stop > 60){ //211
        if(!main.hasClass('site-fix')){
          main.addClass('site-fix').css({
            width: main.parent().width()
          });
        }
      }else {     
        if(main.hasClass('site-fix')){
          main.removeClass('site-fix').css({
            width: 'auto'
          });
        }
      }
      stop = null;
    };
    scroll();
    $(window).on('scroll', scroll);
  }();

  //ʾ��ҳ�����
  $(window).on('scroll', function(){
    /*
    var elemDate = $('.layui-laydate,.layui-colorpicker-main')
    if(elemDate[0]){
      elemDate.each(function(){
        var othis = $(this);
        if(!othis.hasClass('layui-laydate-static')){
          othis.remove();
        }
      });
      $('input').blur();
    }
    */

    var elemTips = $('.layui-table-tips');
    if(elemTips[0]) elemTips.remove();

    if($('.layui-layer')[0]){
      layer.closeAll('tips');
    }
  });
  
  //��������
  layui.code({
    elem: 'pre'
  });
  
  //Ŀ¼
  var siteDir = $('.site-dir');
  if(siteDir[0] && $(window).width() > 750){
    layer.ready(function(){
      layer.open({
        type: 1
        ,content: siteDir
        ,skin: 'layui-layer-dir'
        ,area: 'auto'
        ,maxHeight: $(window).height() - 300
        ,title: 'Ŀ¼'
        ,closeBtn: false
        ,offset: 'r'
        ,shade: false
        ,success: function(layero, index){
          layer.style(index, {
            marginLeft: -15
          });
        }
      });
    });
    siteDir.find('li').on('click', function(){
      var othis = $(this);
      othis.find('a').addClass('layui-this');
      othis.siblings().find('a').removeClass('layui-this');
    });
  }

  //��textarea���㴦�����ַ�
  var focusInsert = function(str){
    var start = this.selectionStart
    ,end = this.selectionEnd
    ,offset = start + str.length

    this.value = this.value.substring(0, start) + str + this.value.substring(end);
    this.setSelectionRange(offset, offset);
  };

  //��ʾҳ��
  $('body').on('keydown', '#LAY_editor, .site-demo-text', function(e){
    var key = e.keyCode;
    if(key === 9 && window.getSelection){
      e.preventDefault();
      focusInsert.call(this, '  ');
    }
  });

  var editor = $('#LAY_editor')
  ,iframeElem = $('#LAY_demo')
  ,runCodes = function(){
    if(!iframeElem[0]) return;
    var html = editor.val();

    var iframeDocument = iframeElem.prop('contentWindow').document;
    iframeDocument.open();
    iframeDocument.write(html);
    iframeDocument.close();

  };
  $('#LAY_demo_run').on('click', runCodes), runCodes();

  //�õ��������λ��
  var setScrollTop = function(thisItem, elemScroll){
    if(thisItem[0]){
      var itemTop = thisItem.offset().top
      ,winHeight = $(window).height();
      if(itemTop > winHeight - 160){
        elemScroll.animate({'scrollTop': itemTop - winHeight/2}, 200);
      }
    }
  }

  //��ѡ�еĲ˵������ڿ��ӷ�Χ��
  util.toVisibleArea({
    scrollElem: $('.layui-side-scroll').eq(0)
    ,thisElem: $('.site-demo-nav').find('dd.layui-this')
  });

  util.toVisibleArea({
    scrollElem: $('.layui-side-scroll').eq(1)
    ,thisElem: $('.site-demo-table-nav').find('li.layui-this')
  });
  


  //�鿴����
  $(function(){
    var DemoCode = $('#LAY_democode');
    DemoCode.val([
      DemoCode.val()
      ,'<body>'
      ,global.preview
      ,'\n<script src="//res.layui.com/layui/dist/layui.js" charset="utf-8"></script>'
      ,'\n<!-- ע�⣺�����ֱ�Ӹ������д��뵽���أ����� JS ·����Ҫ�ĳ��㱾�ص� -->'
      ,$('#LAY_democodejs').html()
      ,'\n</body>\n</html>'
    ].join(''));
  });

  //����鿴����ѡ��
  element.on('tab(demoTitle)', function(obj){
    if(obj.index === 1){
      if(device.ie && device.ie < 9){
        layer.alert('ǿ�Ҳ��Ƽ���ͨ��ie8/9 �鿴���룡��Ϊ�����еı�ǩ���ᱻ��ʽ�ɴ�д����û�л��з���Ӱ���Ķ�');
      }
    }
  })


  //�ֻ��豸�ļ�����
  var treeMobile = $('.site-tree-mobile')
  ,shadeMobile = $('.site-mobile-shade')

  treeMobile.on('click', function(){
    $('body').addClass('site-mobile');
  });

  shadeMobile.on('click', function(){
    $('body').removeClass('site-mobile');
  });



  //���˽�
  ;!function(){
    if(elemHome.data('date') === '4-1'){
      return
      if(local['20180401']) return;

      elemHome.addClass('site-out-up');
      setTimeout(function(){
        layer.photos({
          photos: {
            "data": [{
              "src": "//cdn.layui.com/upload/2018_4/168_1522515820513_397.png",
            }]
          }
          ,anim: 2
          ,shade: 1
          ,move: false
          ,end: function(){
            layer.msg('�޹��������ѣ�', {
              shade: 1
            }, function(){
              layui.data('layui', {
                key: '20180401'
                ,value: true
              });
            });
          }
          ,success: function(layero, index){
            elemHome.removeClass('site-out-up');

            layero.find('#layui-layer-photos').on('click', function(){
              layer.close(layero.attr('times'));
            }).find('.layui-layer-imgsee').remove();
          }
        });
      }, 1000*3);
    }
  }();

  //��ȡ�ĵ�ͼ������
  home.getIconData = function(){
    var $ = layui.$
    ,iconData = []
    ,iconListElem = $('.site-doc-icon li');

    iconListElem.each(function(){
      var othis = $(this);
      iconData.push({
        title: $.trim(othis.find('.doc-icon-name').text())
        ,fontclass: $.trim(othis.find('.doc-icon-fontclass').text())
        ,unicode: $.trim(othis.find('.doc-icon-code').html())
      });
    });
    
    $('.site-h1').html('<textarea style="width: 100%; height: 600px;">'+ JSON.stringify(iconData) + '</textarea>');
  };


  exports('global', home);
});