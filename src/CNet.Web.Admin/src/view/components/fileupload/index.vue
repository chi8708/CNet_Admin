<template>
    <div>
      <!-- <Form ref="fromFile" label-position="right" :label-width="60" inline>
        <FormItem label="上传到文件夹" :label-width="100">
          <Select v-model="fileDirName" style="width:200px" @on-change="fromFileChange">
            <Option v-for="(item,index) in DirType" :value="item.value" :key="index">{{item.label}}</Option>
          </Select>
        </FormItem>
      </Form> -->
      
      <Upload :before-upload="handleUpload" :action="''">
        <Button icon="ios-cloud-upload-outline">选择要上传的文件</Button>
      </Upload>
      <div style="margin: 10px 0;" v-if="file !== null"><span style="color: darkred;">要上传的文件: </span>{{ file.name }}
      </div>
      <div>
        <Button type="primary" @click="upload"
        :loading="loadingStatus">{{ loadingStatus ? 'Uploading' : '确认上传' }}</Button>
      <Button style="margin-left: 10px;" v-if="file !== null" type="warning" @click="clearFile">清除文件</Button>
       <span> 已上传:{{this.uploadFileUrl_Success}}</span>
      </div>

    </div>
  </template>
  
  <script>
    import {
      uploadFile,
      mergeFile
    } from "@/api/uploadFile";
  
    export default {
      props: {
        dirType:String
      },
      data() {
        return {
          //上传到哪个文件夹
          DirType: [{
              value: '',
              label: ''
            }
          ],
          uploadFileUrl_Success:'无',
          fileDirName: '',
          fileData: [],
          fileStatus: '',
          uploadFileLoading: false,
          uploadFileNum: 0,
  
          file: null,
          loadingStatus: false
        }
      },
      mounted() {
      },
      methods: {
        fileStatusChange(e) {
          this.fileStatus = e
        },
        handleUpload(file) {
          this.file = file
          return false;
        },
        upload() {
          // if (this.fileDirName == '') {
          //   this.$Message.error("需要上传至的文件夹为空，请选择")
          //   return
          // }
          if (this.file == null) {
            this.$Message.error("请先选择要上传的文件");
            return
          }
          this.loadingStatus = true
          this.fileChunk(this.file)
        },
        clearFile() {
          this.file = null
        },
  
        fromFileChange(e) {
          console.log(e)
          this.fileDirName = e
        },
  
        // 文件切片
        fileChunk(file) {
          // let name = new Date().getTime() + '' + (Math.floor(Math.random() * (9999 - 1000)) + 1000);
          let fileName = file.name
          let fileType = `${file.name.split('.')[1]}`
          let chunkSize = 1 * 1024 * 1024 // 1M
          //控制切片大小
          if (file.size > (1 * 1024 * 1024) && file.size <= (10 * 1024 * 1024)) {
            chunkSize = 1 * 1024 * 1024
          }
          if (file.size > (10 * 1024 * 1024) && file.size <= (100 * 1024 * 1024)) {
            chunkSize = 10 * 1024 * 1024
          }
          if (file.size > (100 * 1024 * 1024)) {
            chunkSize = 100 * 1024 * 1024
          }
          let chunkList = [] // 每一片文件流数据
          if (file.size < chunkSize) { //文件小于1M就只有一片，不用切
            chunkList.push(file.slice(0))
          } else {
            var start = 0,end = 0
            while (true) {
              end += chunkSize // 结束为止 = 结束位置 + 每片大小
              let blob = file.slice(start, end) // 文件流从开始位置截取到结束位置
              start += chunkSize
              if (!blob.size) {
                break;
              }
              chunkList.push(blob)
            }
          }
          this.transformFileType(chunkList, fileName, fileType) // 文件类型转换
        },
  
        // 文件类型转换
        async transformFileType(list, name, type) {
          this.uploadFileNum = 0 // 已经上传的文件数量
          // let num = localStorage.getItem("uploadFileNum") ? localStorage.getItem("uploadFileNum") : 0 // 已经上传文件数量
          let num = this.uploadFileNum // 已经上传文件数量
          // 如果已经全部上传 直接合并
          // if (num == list.length) return this.mergeFileFun(name, list.length, type)
          //刚开始num为0
          for (var i = num; i < list.length; i++) {
            // 文件转换-异步任务
            let transToFile = async (blob, fileName, fileType) => {
              return new window.File([blob], fileName, {
                type: fileType
              })
            }
            let fileFormData = new FormData()
            const transToFileRes = await transToFile(list[i], name, type)
            fileFormData.append('file', transToFileRes) //文件内容
            fileFormData.append('fileName', name) //文件名称
            fileFormData.append('fileType', type) //文件名称
            fileFormData.append('fileNum', Number(i) + 1) //文件次数
            this.uploadFileMethods(fileFormData, i, name, list, type) // 上传文件
          }
        },
  
        // 上传文件
        uploadFileMethods(fileFormData, uploadFileNum, name, list, type) {
          uploadFile(fileFormData).then(res => {
              this.uploadFileNum = this.uploadFileNum + 1
              // 如果已经上传数量uploadFileNum与切片长度一致，就开始合并文件
              if (this.uploadFileNum == list.length) {
                this.mergeFileFun(name, list.length, type)
                //this.$emit('UpLoadFileUrl', name);
                this.uploadFileLoading = false;
                this.uploadFileNum = 0
              }
            })
            .catch(err => {
              this.uploadFileNum = 0
            });
        },
        
      // 合并文件
      mergeFileFun(name, fileNo, type) {
        let fileFormData = new FormData() // 合并文件接口参数
        fileFormData.append('fileName', name) // 合并的该套文件的文件名
        fileFormData.append('fileTotalNum', fileNo) // 该套文件总数量
        fileFormData.append('fileSuffix', type) // 文件类型后缀
        fileFormData.append('fileDirName', this.fileDirName) //文件夹名

        mergeFile(fileFormData).then(res => {
          const {
            data
          } = res
          if (data.code == 1) {
            this.loadingStatus = false;
            this.file = null
            this.$Message.success(data.msg);
            this.uploadFileUrl_Success=data.data.newFileName;

          } else {
            this.$Message.error(data.msg)
          }
          this.uploadFileNum = 0
          this.modelAdd = false
          //向父组件传值
          this.$emit('UpLoadFileUrl', data.data);

        }).catch(err => {
          this.uploadFileNum = 0
          this.modelAdd = false;
        });
      },
  
      },
      mounted(){
        this.fileDirName=this.dirType;
      }
    }
  </script>
  
  <style>
  </style>
  