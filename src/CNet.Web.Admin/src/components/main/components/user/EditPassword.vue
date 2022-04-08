<template>
  <div>
    <Form ref="formInline" label-position="right" :model="Row" :rules="rule" :label-width="100">
      <Row>
        <Col span="24">
         
          <FormItem label="用户名">
              <label>{{this.$store.state.user.userCode}}</label>
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="原密码" prop='OldPassword'>
            <Input type="password" v-model="Row.OldPassword" />
          </FormItem>
        </Col>
      </Row>
     <Row>
        <Col span="24">
          <FormItem label="新密码" prop='Password' >
            <Input type="password" v-model="Row.Password" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="重复新密码" prop='Password2'>
            <Input type="password" v-model="Row.Password2" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <div style="text-align:center;">
            <Button @click="parent.modelEditPassword=false">取消</Button>
            <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
          </div>
        </Col>
      </Row>
    </Form>
  </div>
</template>
<script>
import { editPassword } from "@/api/pubUser";
export default {
  props: { editRow: Object, parent: Object },
  name:'EditPassword',
  computed: {},
  data() {
    return {
      Row: {},
      rule: {
        OldPassword: [
          {
            required: true,
            message: "密码长度6-18位",
            trigger: "blur",
            min:6,
            max:18
          }
        ],
        Password: [
          {
            required: true,
            message: "密码长度6-18位",
            trigger: "blur",
            min:6,
            max:18
          }
         ],
         Password2:[
          {
            required: true,
            message: "密码长度6-18位",
            trigger: "blur",
            min:6,
            max:18
          }
        ]
      }
    };
  },
  methods: {
    save() {
        this.saveEdit();
    },
    saveEdit() {
      this.saveValidate().then(r => {
        if (r) {
           if(this.Row.Password!=this.Row.Password2){
                this.$Message.info("新密码和重复密码不一致");
               return;
           }
          editPassword(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("编辑成功");
                this.parent.modelEditPassword = false;
                this.parent.logout();
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
    }
  },
  mounted() {
  }
};
</script>

