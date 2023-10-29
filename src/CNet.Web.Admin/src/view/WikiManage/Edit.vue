<template>
  <div>
    <!-- Id, UserCode, UserName, RealName, UserPwd, Sex, IdentityNo, Birthday, WikiMainCode, ManagerFlag, Tel, EMail, QQ, Remark, StopFlag, Crid, Crdt, Lmid, Lmdt, LoginDate, ProvinceCode, CityCode, RegionCode, UserAddress, Wxcode, HeadUrl -->
    <Form ref="formInline" label-position="right" :model="Row" :rules="rule" :label-width="100">
      <Row>
        <Col span="24">
        <FormItem label="类别" prop="parentName" id="item-parentName">
          <Input search enter-button v-model="Row.parentName" readonly @on-search="wikiMainSelect" />
          <!-- <Tree :data="WikiMainTree"></Tree> -->
          <!-- <Input type="password" v-model="Row.wikiMainCode" /> -->
        </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
        <FormItem label="文件">
          <upLoadFile @UpLoadFileUrl="UpLoadFileUrl" dirType="wiki"></upLoadFile>
        </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
        <FormItem label="名称" prop="title">
          <Input v-model="Row.title" />
        </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
        <FormItem label="备注" prop="Remark">
          <Input v-model="Row.remark" type="textarea" :autosize="{ minRows: 2, maxRows: 2 }" />
        </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
        <div style="text-align:center;">
          <Button @click="parent.modelEdit = false">取消</Button>
          <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
        </div>
        </Col>
      </Row>
    </Form>

    <Modal title="选择部门" :mask-closable="false" v-model="modelWikiMain" width="300" scrollable footer-hide>
      <sort-tree :parent="this"></sort-tree>
    </Modal>
  </div>
</template>
<script>
import upLoadFile from '../components/fileupload/index.vue'
import { add, edit } from "@/api/wikiMain";
import SortTree from "./SortTree";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  components: {
    SortTree,
    upLoadFile
  },
  data() {
    return {
      Row: {},
      modelWikiMain: false,
      rule: {
        parentName: [
          {
            required: true,
            message: "类别必选",
            trigger: "blur",
            validator: (rule, value, callback, source, options) => {
              if (!this.Row.parentName) {
                return callback(new Error('请选择一个类别'));
              } else {
                callback(); return;
              }
            },
          }
        ],
        title: [
          {
            required: true,
            message: "名称必填",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    wikiMainSelect() {
      this.modelWikiMain = true;
    },
    sortChange(code, title) {
      this.Row.parentCode = code;
      this.Row.parentName = title;
      this.$refs.formInline.validateField('parentName');
      this.modelWikiMain = false;
    },
    save() {
      if (this.parent.isAdd) {
        this.saveAdd();
      } else {
        this.saveEdit();
      }
    },
    saveAdd() {
      this.saveValidate().then(r => {
        if (r) {
          add(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("添加成功");
                this.parent.modelEdit = false;
                this.parent.reloadAll(this.Row.parentCode);
              } else {
                this.$Message.error({
                  content: msg,
                  duration: 10,
                  closable: true
                });
              }
            })
            .catch(err => { });
        }
      });
    },
    saveEdit() {
      this.saveValidate().then(r => {
        if (r) {
          edit(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("编辑成功");
                this.parent.modelEdit = false;
                this.parent.reloadAll(this.Row.parentCode);
              } else {
                this.$Message.error({
                  content: msg,
                  duration: 10,
                  closable: true
                });
              }
            })
            .catch(err => { });
        }
      });
    },
    saveValidate(name = "formInline") {
      return this.$refs[name].validate(valid => {
        if (!valid) {
          this.$Message.warning("请检查表单数据！");
          return false;
        } else {
          return true;
        }
      });
    },
    handleReset(name = "formInline") {
      this.$refs[name].resetFields();
    },
    //上传文件回调
    UpLoadFileUrl(e) {
      this.Row.title = e.fileName;
      this.Row.filePath=e.newFileUrl+"\\"+e.newFileName;
      //this.$set(this.Row,"title",e.fileName);
      console.log(e);
    }
  },
  watch: {
    // editRow(newVal, oldVal) {
    //   this.Row = Object.assign({}, newVal);
    // }
  },
  mounted() {
    this.Row = Object.assign({title:'',filePath:''}, this.editRow)
  }
}
</script>

