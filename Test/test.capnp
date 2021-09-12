@0xb258be8f1a9cf184;

interface TestPayloadService {
	log @0 (message :Text);
	echo @1 (message :Text) -> (message :Text);
}
