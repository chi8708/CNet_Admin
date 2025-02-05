

        <!-- 此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-02-05 17:39:05 -->
    
<template>
  <div>
    <Form ref="formInline" label-position="right" :model="Row" :rules="rule" :label-width="100">
              <Row>
                <Col span="24">
                <FormItem label="创建时间" prop="Crdt">
                    <Input v-model="Row.Crdt" />
                </FormItem>
                </Col>
             </Row>
              <Row>
                <Col span="24">
                <FormItem label="创建人" prop="Crid">
                    <Input v-model="Row.Crid" />
                </FormItem>
                </Col>
             </Row>
              <Row>
                <Col span="24">
                <FormItem label="" prop="Id">
                    <Input v-model="Row.Id" />
                </FormItem>
                </Col>
             </Row>
              <Row>
                <Col span="24">
                <FormItem label="最后更新时间" prop="Lmdt">
                    <Input v-model="Row.Lmdt" />
                </FormItem>
                </Col>
             </Row>
              <Row>
                <Col span="24">
                <FormItem label="最后更新人" prop="Lmid">
                    <Input v-model="Row.Lmid" />
                </FormItem>
                </Col>
             </Row>
              <Row>
                <Col span="24">
                <FormItem label="备注" prop="Remark">
                    <Input v-model="Row.Remark" />
                </FormItem>
                </Col>
             </Row>
              <Row>
                <Col span="24">
                <FormItem label="角色编号" prop="RoleCode">
                    <Input v-model="Row.RoleCode" />
                </FormItem>
                </Col>
             </Row>
              <Row>
                <Col span="24">
                <FormItem label="角色名称" prop="RoleName">
                    <Input v-model="Row.RoleName" />
                </FormItem>
                </Col>
             </Row>
              <Row>
                <Col span="24">
                <FormItem label="停用状态 默认0  未停用 1 停用" prop="StopFlag">
                    <Input v-model="Row.StopFlag" />
                </FormItem>
                </Col>
             </Row>
            <Row>
                <Col span="24">
                <div style="text-align:center;">
                    <Button @click="parent.modelEdit=false">取消</Button>
                    <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
                </div>
                </Col>
            </Row>
    </Form>
  </div>
</template>
<script>
import { add , edit } from "@/api/pubRole";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  data() {
    return {
      Row: {},
      rule: {
        roleName: [
          {
            required: true,
            message: "角色名必填",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
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
                this.parent.setPageData(1);
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
                this.parent.setPageData();
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
    }
  },
  watch: {
    editRow(newVal, oldVal) {
      this.Row = Object.assign({}, newVal);
      this.Row.sex = this.Row.sex === false ? 0 : 1;
    }
  },
  mounted() {
  }
};
</script>

