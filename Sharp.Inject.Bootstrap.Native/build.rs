fn main() {
    ::capnpc::CompilerCommand::new()
        .src_prefix("../Sharp.Inject.Rpc")
        .file("../Sharp.Inject.Rpc/sharp_inject_native.capnp")
        .run()
        .unwrap();
}
