<template>
  <div style="border: 1px solid #ccc;">
    <Toolbar style="border-bottom: 1px solid #ccc" :editor="editor" :defaultConfig="toolbarConfig" :mode="mode" />
    <Editor style="height: 500px; overflow-y: hidden;" v-model="html" :defaultConfig="editorConfig" :mode="mode"
      @onCreated="onCreated" @onChange="onChange" />
  </div>
</template>

<script>
  import Vue from 'vue'
  import {
    Editor,
    Toolbar
  } from '@wangeditor/editor-for-vue'
  import config from "@/config";
  export default Vue.extend({
    components: {
      Editor,
      Toolbar
    },
    props: {
      parent: Object,
      EditContent: String
    },
    data() {
      var token = this.$store.state.user.token;
      return {
        editor: null,
        html: '',
        toolbarConfig: {
          excludeKeys: [
            // 排除菜单组，写菜单组 key 的值即可
            'uploadVideo', //去掉视频
            "fullScreen"
          ],
        },
        editorConfig: {
          placeholder: '请输入内容...',
          MENU_CONF: {
            uploadImage: {
              server: config.imgurl + "api/UpLoadFile/UploadInterface/wangeditor",
              // form-data fieldName ，默认值 'wangeditor-uploaded-image'
              fieldName: 'your-custom-name',
              // 单个文件的最大体积限制，默认为 2M
              maxFileSize: 1 * 1024 * 1024, // 1M
              // 最多可上传几个文件，默认为 100
              //maxNumberOfFiles: 10,
              // 选择文件时的类型限制，默认为 ['image/*'] 。如不想限制，则设置为 []
              allowedFileTypes: ["image/png,image/jpeg,image/jpg"],
              // 自定义上传参数，例如传递验证的 token 等。参数会被添加到 formData 中，一起上传到服务端。
              meta: {},
              // 将 meta 拼接到 url 参数中，默认 false
              metaWithUrl: false,
              // 自定义增加 http  header
              headers: {
                Authorization: "Bearer " + token
              },
              // 跨域是否传递 cookie ，默认为 false
              withCredentials: false,
              // 超时时间，默认为 10 秒
              timeout: 5 * 1000, // 5 秒
              onBeforeUpload(file) {
                let fileObj = Object.values(file)[0].data;
                return file
              },
              customInsert(res, insertFn) {
                let url = config.imgurl + "FileUpload/" + res.data[0].fileUrl;
                insertFn(url, res.data[0].fileName, url)
              },
              onProgress(progress) {
                //console.log('progress', progress)
              },
              onError: (file, err, res) => {
                if (err.message.indexOf("exceeds maximum allowed size of 1 MB") > -1) {
                  this.$Message.error({
                    content: "上传图片小于1M!",
                    duration: 5,
                    closable: true,
                  });
                }
                console.log("[ err ] >", err);
              },
            },
          },
        },
        mode: 'default', // or 'simple'
      }
    },
    methods: {
      onCreated(editor) {
        this.editor = Object.seal(editor) // 一定要用 Object.seal() ，否则会报错
        this.html = this.EditContent;
      },
      onChange(editor) {
        this.parent.WangEditorCallback(editor.getHtml(), editor.getText())
      },
    },
    mounted() {

    },
    beforeDestroy() {
      const editor = this.editor
      if (editor == null) return
      editor.destroy() // 组件销毁时，及时销毁编辑器
    }
  })
</script>
<style src="@wangeditor/editor/dist/css/style.css"></style>
