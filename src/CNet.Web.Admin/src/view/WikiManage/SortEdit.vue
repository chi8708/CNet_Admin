<template>
    <div>
      <!-- Id, UserCode, UserName, RealName, UserPwd, Sex, IdentityNo, Birthday, SortCode, ManagerFlag, Tel, EMail, QQ, Remark, StopFlag, Crid, Crdt, Lmid, Lmdt, LoginDate, ProvinceCode, CityCode, RegionCode, UserAddress, Wxcode, HeadUrl -->
      <Form ref="formInline" label-position="right" :model="Row" :rules="rule" :label-width="100">
        <Row>
          <Col span="24">
            <FormItem label="父级" prop="parentName" id="item-parentName">
                <Row>
                    <Col span="22">
                        <Input search enter-button v-model="Row.parentName" readonly @on-search="sortSelect" />
                    </Col>
                    <Col span="2" >
                        <Button  icon="ios-close-circle-outline" @click="clearParent()"></Button>
                    </Col>
                </Row>
              <!-- <Tree :data="SortTree"></Tree> -->
              <!-- <Input type="password" v-model="Row.sortCode" /> -->
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="24">
            <FormItem label="名称" prop="sortName" >
              <Input v-model="Row.sortName" />
            </FormItem>
          </Col>
        </Row>
        <!-- <Row>
          <Col span="24">
            <FormItem label="备注" prop="Remark">
              <Input v-model="Row.remark" type="textarea" :autosize="{minRows: 2,maxRows: 2}" />
            </FormItem>
          </Col>
        </Row> -->
        <Row>
          <Col span="24">
            <div style="text-align:center;">
              <Button @click="parent.modelEdit=false">取消</Button>
              <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
            </div>
          </Col>
        </Row>
      </Form>
  
      <Modal
        title="选择分类"
        :mask-closable="false"
        v-model="modelSort"
        width="300"
        scrollable
        footer-hide
      >
      <tree  ref="sortTree" :parent="this" :isManage="false"></tree>
      </Modal>
    </div>
  </template>
  <script>
  import {add, edit } from "@/api/wikiSort";
//   import SortTree from "./SortTree.vue";
  export default {
    name:'sort-edit',
    props: { editRow: Object, parent: Object },
    computed: {},
    components: {
        tree: () => import('./SortTree.vue')
    },
    data() {
      return {
        Row: {},
        Roles: [],
        Sorts: [],
        SortTree: [],
        SortTreeItems: [],
        modelSort: false,
        rule: {
          parentName: [
            // {
            //   required: true,
            //   message: "父级必选",
            //   trigger: "blur",
            //   validator: (rule, value, callback, source, options) => {
            //     if (!this.Row.parentName) {
            //       return callback(new Error('请选择一个父级'));
            //     } else {
            //       callback(); return;
            //     }
            //   },
            // }
          ],
          sortName: [
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
      sortSelect() {
        this.modelSort = true;
      },
      sortChange(code, title) {
        this.Row.parentCode = code;
        this.Row.parentName = title;
        this.$refs.formInline.validateField('parentName');
        this.modelSort = false;
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
                  this.parent.getSorts(this.Row.parentCode);
                } else {
                  this.$Message.error({
                    content: msg,
                    duration: 10,
                    closable: true
                  });
                }
              })
              .catch(err => {});
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
                  this.parent.getSorts(this.Row.parentCode);
                } else {
                  this.$Message.error({
                    content: msg,
                    duration: 10,
                    closable: true
                  });
                }
              })
              .catch(err => {});
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
      clearParent(){
        this.Row.parentCode="";
        this.Row.parentName="";
        //this.$set(this.Row,'parentName','');
        //this.$forceUpdate();
        //console.log(this.Row);
      }
    },
    watch: {
    },
    mounted() {
        //新增时parentName 响应式需要添加默认属性。或者通过this.$forceUpdate()强制更新
        this.Row = Object.assign({'parentName':''}, this.editRow);
    }
  };
  </script>
  
  