<template>
<!-- <div id="myChart" :style="{width: '300px', height: '300px'}"></div> -->
<WangEditor ref="WangEditor" :parent="this" :EditContent="msg"></WangEditor>
</template>
<script>
  import WangEditor from '../components/wangEditor/Index.vue'
export default {
  name: 'hello',
  components:{
    WangEditor
  },
  data () {
    return {
      msg: 'Welcome to Your Vue.js App'
    }
  },
  mounted(){
    //this.drawLine();
  },
  methods: {
    WangEditorCallback(html, text) {
        this.msg = html;
        this.msg = text
    },
    drawLine(){
        // 基于准备好的dom，初始化echarts实例
        let myChart = this.$echarts.init(document.getElementById('myChart'))
        // 绘制图表
        myChart.setOption({
            title: { text: '在Vue中使用echarts' },
            tooltip: {},
            xAxis: {
                data: ["衬衫","羊毛衫","雪纺衫","裤子","高跟鞋","袜子"]
            },
            yAxis: {},
            series: [{
                name: '销量',
                type: 'bar',
                data: [5, 20, 36, 10, 10, 20]
            }]
        });
    }
  }
}
</script>
