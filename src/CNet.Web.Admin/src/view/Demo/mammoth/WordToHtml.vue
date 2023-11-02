<template>
    <div class="home">
      <!-- 样式会乱 不好用 -->
        <div class="word-box" id="wordView" v-html="vHtml" />
    </div>
</template>
  <script>
  import mammoth from 'mammoth';
  
  export default {
    data() {
      return {
        vHtml: '',
      };
    },
    mounted() {
      // 调用函数来执行文档转换
      this.previewWord("http://localhost:10910/api/WikiMain/GetPdfFile?filePath=/FileUpload/wiki/1.pdf");
    },
    methods: {
        previewWord(url) {
            var vm = this
            const xhr = new XMLHttpRequest()
            xhr.open('get', url, true)
            xhr.responseType = 'arraybuffer'
            xhr.onload = function () {
                if (xhr.status === 200) {
                    mammoth.convertToHtml({ arrayBuffer: new Uint8Array(xhr.response) }).then(function (resultObject) {
                        vm.$nextTick(() => {
                            vm.vHtml = resultObject.value
                        })
                    })
                }
            }
            xhr.send()
        },
    },
  };
  </script>
  