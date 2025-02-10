

        <!-- 此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-02-10 15:50:07 -->
    
<template>
  <div>
    <Form ref="formInline" label-position="right" :model="Row" :rules="rule" :label-width="100">
      <FormItem label="角色编号" prop="roleCode">
        <Row>
          <Col span="24">
            <Input v-model="Row.roleCode" />
        </Col>
       </Row>
      </FormItem>
      <FormItem label="角色名称" prop="roleName">
        <Row>
          <Col span="24">
            <Input v-model="Row.roleName" />
        </Col>
       </Row>
      </FormItem>
      <FormItem label="备注" prop="remark">
        <Row>
          <Col span="24">
            <Input v-model="Row.remark" />
        </Col>
       </Row>
      </FormItem>
      <FormItem label="停用状态 默认0  未停用 1 停用" prop="stopFlag">
        <Row>
          <Col span="24">
            <i-switch v-model="Row.stopFlag" size="large">
                <template #open><span>是</span></template>
                <template #close><span>否</span></template>
            </i-switch>
        </Col>
       </Row>
      </FormItem>
      <FormItem label="创建人" prop="crid">
        <Row>
          <Col span="24">
            <Input v-model="Row.crid" />
        </Col>
       </Row>
      </FormItem>
      <FormItem label="创建时间" prop="crdt">
        <Row>
          <Col span="24">
            <DatePicker type="date" placeholder="请选择日期" v-model="Row.crdt"></DatePicker>
        </Col>
       </Row>
      </FormItem>
      <FormItem label="最后更新人" prop="lmid">
        <Row>
          <Col span="24">
            <Input v-model="Row.lmid" />
        </Col>
       </Row>
      </FormItem>
      <FormItem label="最后更新时间" prop="lmdt">
        <Row>
          <Col span="24">
            <DatePicker type="date" placeholder="请选择日期" v-model="Row.lmdt"></DatePicker>
        </Col>
       </Row>
      </FormItem>
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
import { add , edit } from "@/api/Pub_Role";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  data() {
    return {
      Row: {},
      rule: {

            roleCode: [
               {
                required: true,
                message: "角色编号 必填",
                trigger: "blur"
                }
            ],
        
            roleName: [
               {
                required: true,
                message: "角色名称 必填",
                trigger: "blur"
                }
            ],
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

